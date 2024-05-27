using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TM.Website.Filters
{
    public class LoggedUserFilter : ActionFilterAttribute
    {
        //If the User is not logged
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Session.GetInt32("loggedUserId").HasValue)
            {
                context.HttpContext.Response.Redirect("/Users/Login");
                context.Result = new EmptyResult();
            }
            base.OnActionExecuting(context);
        }
    }
}
