using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CampusHiring.Api.AuthorizationFilter;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class InterviewerOrSystemAdminAttribute : TypeFilterAttribute
{
    public InterviewerOrSystemAdminAttribute() : base(typeof(InterviewerOrSystemAdminFilter))
    {

    }
}

public class InterviewerOrSystemAdminFilter(CampusHiringDbContext dbcontext) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpUser = context.HttpContext.User;

        if (httpUser.Identity?.IsAuthenticated == false)
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

        context.RouteData.Values.TryGetValue("companyId", out var companyIdObj);

        int.TryParse(companyIdObj?.ToString(), out var companyId);

        if (companyId == 0)
        {
            context.Result = new ForbidResult();
            return;
        }

        var isCompanyInterviewer = await dbcontext.Interviewers.AnyAsync(i => i.CompanyId == companyId && i.UserId == userId);

        if (!isCompanyInterviewer)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
