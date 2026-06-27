using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CampusHiring.Api.AuthorizationFilter;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class CollegeOrSystemAdminAttribute : TypeFilterAttribute
{
    public CollegeOrSystemAdminAttribute() : base(typeof(CollegeOrSystemAdminFilter))
    {

    }
}

public class CollegeOrSystemAdminFilter(CampusHiringDbContext dbcontext) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpUser = context.HttpContext.User;

        if(httpUser.Identity?.IsAuthenticated == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (httpUser.IsInRole(RoleNames.Admin))
        {
            return;
        }

        var userId = httpUser.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? httpUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.RouteData.Values.TryGetValue("collegeId", out var collegeIdObj);

        int.TryParse(collegeIdObj?.ToString(), out var collegeId);

        if(collegeId == 0)
        {
            context.Result = new ForbidResult();
            return;
        }

        var isCollegeAdmin = await dbcontext.CollegeAdmins.AnyAsync(c =>  c.Id == collegeId && c.UserId == userId);
        if (!isCollegeAdmin)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}