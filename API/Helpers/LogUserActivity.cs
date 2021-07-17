using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (resultContext.HttpContext.User.Identity is {IsAuthenticated: false}) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            if (uow != null)
            {
                var user = await uow.UserRepository.GetUserByIdAsync(userId);
                user.LastActive = DateTime.UtcNow;
                Console.WriteLine("Last active: " + user.LastActive);
                await uow.Complete();
            }
        }
    }
}