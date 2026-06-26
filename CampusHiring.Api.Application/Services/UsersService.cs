using AutoMapper;
using CampusHiring.Api.Application.DTOs.Auth;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Error = CampusHiring.Api.Common.Results.Error;

namespace CampusHiring.Api.Application.Services;

public class UsersService(UserManager<User> userManager, CampusHiringDbContext context, IMapper mapper)
{
    public async Task<Result<RegisteredUserDto>> RegisterUserAsync(RegisterUserDto userDto)
    {
        var user = new User
        {
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
        };
        var result = await userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(ErrorCodes.BadRequest,e.Description)).ToArray();
            return Result<RegisteredUserDto>.BadRequest(errors);
        }

        await userManager.AddToRoleAsync(user, userDto.Role);

        if (userDto.Role == RoleNames.Student)
        {
            var college = await context.Colleges.FindAsync(userDto.AssociatedCollegeId);
            if(college == null)
            {
                return Result<RegisteredUserDto>.NotFound(new Error(ErrorCodes.NotFound, $"College Id {userDto.AssociatedCollegeId} is not found"));
            }

            var student = mapper.Map<Student>(userDto);
            student.UserId = user.Id;
            context.Students.Add(student);
            await context.SaveChangesAsync();
        }

        if(userDto.Role == RoleNames.Interviewer)
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

        if(userDto.Role == RoleNames.CollegeAdmin)
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
}
