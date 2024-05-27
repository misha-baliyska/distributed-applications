using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TM.Website.Filters
{
    public class UserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool IsAdmin = AuthUser.LoggedUser.IsAdmin;
            if (IsAdmin)
            {
                context.HttpContext.Response.Redirect("/Home/Index");
                context.Result = new EmptyResult();
            }
            base.OnActionExecuting(context);
        }
    }
}
