using Inlämningsuppgift_1.Core.Interfaces;
using Inlämningsuppgift_1.Core.Services;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift_1.Data.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Inlämningsuppgift_1.Core.Extensions
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjectionSetup(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDatabase>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}
