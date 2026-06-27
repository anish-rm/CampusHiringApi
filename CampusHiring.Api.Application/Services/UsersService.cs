using AutoMapper;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Auth;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Models.Config;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Error = CampusHiring.Api.Common.Results.Error;

namespace CampusHiring.Api.Application.Services;

public class UsersService(UserManager<User> userManager, CampusHiringDbContext context, IMapper mapper, IOptions<JwtSettings> jwtOptions) : IUsersService
{
    public async Task<Result<RegisteredUserDto>> RegisterUserAsync(RegisterUserDto userDto)
    {
        var user = new User
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
        };
        var result = await userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(ErrorCodes.BadRequest, e.Description)).ToArray();
            return Result<RegisteredUserDto>.BadRequest(errors);
        }

        await userManager.AddToRoleAsync(user, userDto.Role);

        if (userDto.Role == RoleNames.Student)
        {
            var college = await context.Colleges.FindAsync(userDto.AssociatedCollegeId);
            if (college == null)
            {
                return Result<RegisteredUserDto>.NotFound(new Error(ErrorCodes.NotFound, $"College Id {userDto.AssociatedCollegeId} is not found"));
            }

            var student = mapper.Map<Student>(userDto);
            student.UserId = user.Id;
            context.Students.Add(student);
            await context.SaveChangesAsync();
        }

        if (userDto.Role == RoleNames.Interviewer)
        {
            var company = await context.Companies.FindAsync(userDto.AssociatedCompanyId);

            if (company == null)
            {
                return Result<RegisteredUserDto>.NotFound(new Error(ErrorCodes.NotFound, $"Company Id {userDto.AssociatedCollegeId} is not found"));
            }

            var interviewer = new Interviewer
            {
                Designation = userDto.Designation,
                UserId = user.Id,
            };
            context.Interviewers.Add(interviewer);
            await context.SaveChangesAsync();
        }

        if (userDto.Role == RoleNames.CollegeAdmin)
        {
            var college = await context.Colleges.FindAsync(userDto.AssociatedCollegeId);
            if (college == null)
            {
                return Result<RegisteredUserDto>.NotFound(new Error(ErrorCodes.NotFound, $"College Id {userDto.AssociatedCollegeId} is not found"));
            }

            var collegeAdmin = new CollegeAdmin
            {
                UserId = user.Id,
                CollegeId = college.Id,
            };

            context.CollegeAdmins.Add(collegeAdmin);
            await context.SaveChangesAsync();
        }

        var registeredUserDto = new RegisteredUserDto
        {
            Id = user.Id,
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Role = userDto.Role,
        };
        return Result<RegisteredUserDto>.Success(registeredUserDto);
    }

    public async Task<Result<string>> LoginAsync(LoginUserDto userDto)
    {
        var user = await userManager.FindByEmailAsync(userDto.Email);
        if(user == null)
        {
            return Result<string>.NotFound(new Error(ErrorCodes.NotFound, $"User with email {userDto.Email} not found"));
        }
        var isPasswordValid = await userManager.CheckPasswordAsync(user, userDto.Password);
        if (!isPasswordValid)
        {
            return Result<string>.BadRequest(new Error(ErrorCodes.BadRequest, "Invalid Credentials"));
        }
        var token = await GenerateToken(user);

        return Result<string>.Success(token);
    }

    private async Task<string> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
        };

        var roles = await userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

        claims = claims.Union(roleClaims).ToList();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtOptions.Value.DurationInMinutes)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
