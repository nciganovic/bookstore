using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Security
{
    public class EditOnlyOtherRolesAndClaims : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if (authFilterContext == null) {
                return Task.CompletedTask;
            }

            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string getUrl = authFilterContext.HttpContext.Request.GetDisplayUrl();

            List<string> SplitUrlBySlash = getUrl.Split("/").ToList();

            if (context.User.IsInRole("Admin") && context.User.HasClaim(claim => claim.Type == "Edit role" && claim.Value == "true") && SplitUrlBySlash.Last().ToLower() != loggedInAdminId.ToLower()) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
