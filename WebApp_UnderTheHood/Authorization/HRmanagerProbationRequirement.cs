using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_UnderTheHood.Authorization
{
    public class HRmanagerProbationRequirement : IAuthorizationRequirement 
    {
        public HRmanagerProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }

        public int ProbationMonths { get; }
    }

    public class HRmanagerProbationRequirementHandler : AuthorizationHandler<HRmanagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRmanagerProbationRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmploymentDate"))
                return Task.CompletedTask;

            var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate").Value);

            var period = DateTime.Now - empDate;

            if (period.Days > 30 * requirement.ProbationMonths)
                context.Succeed(requirement);

            return Task.CompletedTask;


        }
    }
}
