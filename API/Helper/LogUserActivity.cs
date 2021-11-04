using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extension;
using API.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helper
{
   public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.getUserId();
            var uow = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await uow.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;
            await uow.SavaAllAsync();
        }
    }
}