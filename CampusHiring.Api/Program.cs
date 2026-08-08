using Asp.Versioning;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.MappingProfiles;
using CampusHiring.Api.Application.Services;
using CampusHiring.Api.CachePolicies;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Models.Config;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting CampusHiring API");

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
    );
    //adding connection string
    var connectionString = builder.Configuration.GetConnectionString("CampusHiringConnectionString");

    Log.Information("Using connection string: {ConnectionString}", connectionString);

    builder.Services.AddDbContext<CampusHiringDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

    // Add services to the container.

    builder.Services.AddIdentityApiEndpoints<User>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<CampusHiringDbContext>();

    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

    if (string.IsNullOrEmpty(jwtSettings.Key))
    {
        Log.Fatal("JWT settings are not properly configured. Please configure key to continue");
        throw new InvalidOperationException("JWT settings are not properly configured. Please configure key to continue");
    }

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddScoped<IAssessmentsService, AssessmentsService>();
    builder.Services.AddScoped<IUsersService, UsersService>();
    builder.Services.AddScoped<ICollegesService, CollegesService>();
    builder.Services.AddScoped<ICompaniesService, CompaniesService>();
    builder.Services.AddScoped<IInterviewsService, InterviewsService>();

    builder.Services.AddAutoMapper(cfg => { }, typeof(AssessmentMappingProfile).Assembly);

    builder.Services.AddControllers()
        .AddNewtonsoftJson()
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

    builder.Services.AddMemoryCache();

    //builder.Services.AddOutputCache();
    builder.Services.AddOutputCache(options =>
    {
        options.AddPolicy(CacheConstants.AuthenticatedUserCachingPolicy, builder =>
        {
            builder.AddPolicy<AuthenticatedUserCachingPolicy>()
            .SetCacheKeyPrefix(CacheConstants.AuthenticatedUserCachingPolicyTag);
        }, true);
    });

    builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter(RateLimitingConstants.FixedPolicy, opt =>
        {
            opt.Window = TimeSpan.FromMinutes(1);
            opt.PermitLimit = 5;
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = 0;
        });

        options.AddPolicy(RateLimitingConstants.PerUserPolicy, context =>
        {
            var username = context.User?.Identity?.Name ?? "anonymous";

            return RateLimitPartition.GetSlidingWindowLimiter(username, _ => new SlidingWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 12,
                SegmentsPerWindow = 6,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
            });
        });

        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 200,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10,
            });
        });

        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.OnRejected = async (context, cancellationToken) =>
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.HttpContext.Response.Headers.RetryAfter = retryAfter.TotalSeconds.ToString();
            }
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.HttpContext.Response.ContentType = "application/json";

            await context.HttpContext.Response.WriteAsJsonAsync(new
            {
                error = "Too many requests",
                message = "Rate limit exceeded",
                retryAfter = retryAfter.TotalSeconds
            }, cancellationToken: cancellationToken);
        };
    });

    builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Campus Hiring Api",
            Description = "Api for managing assessments, interviews in colleges",
            Contact = new OpenApiContact
            {
                Name = "Anish",
                Email = "anishmahi946@gmail.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });

        options.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2",
            Title = "Campus Hiring Api V2",
            Description = "Version 2 Api for managing assessments, interviews in colleges",
        });

        var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlpath = Path.Combine(AppContext.BaseDirectory, xmlfile);
        if (File.Exists(xmlpath))
        {
            options.IncludeXmlComments(xmlpath);
        }

        options.EnableAnnotations();

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the bearer scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        options.ExampleFilters();

        options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
        options.OrderActionsBy(apiDesc => $"{apiDesc.RelativePath}_{apiDesc.HttpMethod}");
    });

    builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();


    builder.Services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy("Application is running"), tags: ["api"])
        .AddDbContextCheck<CampusHiringDbContext>(
        name: "Database",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["db", "sql"]);


    var app = builder.Build();

    //app.UseExceptionHandler();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        options.GetLevel = (httpContext, elapsed, ex) => ex != null
        ? LogEventLevel.Error
        : httpContext.Response.StatusCode >= 500
            ? LogEventLevel.Error
            : httpContext.Response.StatusCode >= 400
                ? LogEventLevel.Warning
                : LogEventLevel.Information;

        options.EnrichDiagnosticContext = (diagonasticContext, httpContext) =>
        {
            diagonasticContext.Set("UserName", httpContext?.User?.Identity?.Name ?? "anonymous");
            diagonasticContext.Set("RemoteIP", httpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown");

            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userId = httpContext.User.FindFirst("sub")?.Value ?? "unknown";
                diagonasticContext.Set("UserId", userId);
            }
        };
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        //app.MapOpenApi();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Campus Hiring Api v1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "Campus Hiring Api v2");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Campus Hiring Api Documentation";
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.ShowExtensions();
            options.EnableValidator();
        });
    }

    app.MapGroup("api/defaultauth").MapIdentityApi<User>();

    app.UseHttpsRedirection();

    app.MapHealthChecks("/healthz", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    duration = entry.Value.Duration.TotalMilliseconds,
                    exception = entry.Value.Exception,
                    data = entry.Value.Data
                }),
                totalDuration = report.TotalDuration.TotalMilliseconds,
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }
    });

    app.MapHealthChecks("/healthz/live", new HealthCheckOptions
    {
        Predicate = _ => false
    });

    app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("db")
    });

    app.UseRateLimiter();

    app.UseAuthorization();

    app.UseOutputCache();


    app.MapControllers();

    Log.Information("CampusHiring API started successfully");

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    Log.Information("Application shutdown");
    Log.CloseAndFlush();
}