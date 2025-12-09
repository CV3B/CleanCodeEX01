using Inlämningsuppgift_1.Data.Entities;
using System.Runtime.CompilerServices;

namespace Inlämningsuppgift_1.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static User GetAuthUser(this HttpContext context)
        {
            return (User)context.Items["User"]!;
        }
    }
}
