using Microsoft.Extensions.DependencyInjection;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Core.Authentication;

namespace Core
{
    public static class CoreDependencies
    {
        public static void InjectDBContextDependencies(this IServiceCollection service, string connString)
        {
            service.AddDbContext<OutfitDBContext>(options => options.UseNpgsql(connString));
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            service.AddScoped<IUserContext, UserContext>();
        }
    }
}
