using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.MappingProfiles;
using CampusHiring.Api.Application.Services;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//adding connection string
var connectionString = builder.Configuration.GetConnectionString("CampusHiringConnectionString");

builder.Services.AddDbContext<CampusHiringDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add services to the container.

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CampusHiringDbContext>();

builder.Services.AddScoped<IAssessmentsService, AssessmentsService>();

builder.Services.AddAutoMapper(cfg => { }, typeof(AssessmentMappingProfile).Assembly);

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGroup("api/defaultauth").MapIdentityApi<User>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
