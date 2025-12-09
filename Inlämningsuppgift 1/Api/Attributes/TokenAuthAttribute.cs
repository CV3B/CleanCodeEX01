using Inlämningsuppgift_1.Core.Queries.Users;
using Inlämningsuppgift_1.Data.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inlämningsuppgift_1.Api.Attributes
{
    public class TokenAuthAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();

            if (!context.HttpContext.Request.Headers.TryGetValue(TokenSettings.TokenName, out var token))
            {
                context.HttpContext.Response.StatusCode = 401;
                return;
            }

            var query = new GetUserByTokenQuery { Token = token! };
            var result = await mediator.Send(query);

            if (!result.IsSuccess || result.Value == null)
            {
                context.HttpContext.Response.StatusCode = 401;
                return;
            }

            context.HttpContext.Items["User"] = result.Value;

            await next();
        }
    }
}
