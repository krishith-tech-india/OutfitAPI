using Microsoft.Extensions.DependencyInjection;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public static class CoreDependencies
    {
        public static void InjectDBContextDependencies(this IServiceCollection service, string connString)
        {
            service.AddDbContext<OutfitDBContext>(options => options.UseNpgsql(connString));
        }
    }
}
