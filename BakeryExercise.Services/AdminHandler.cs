namespace BakeryExercise.Services
{
    using BakeryExercise.EntityFramework;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            var adminClaim = context.User.Claims.FirstOrDefault(c => c.Type == BakeryClaimTypes.BakeryAdmin);            
            if (adminClaim != null && adminClaim.Value == bool.TrueString)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
