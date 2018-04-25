using MemberManagement.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace MemberManagement.Api.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = string.Join(" | ", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                throw new ApplicationValidationException(message);
            }
        }
    }
}
