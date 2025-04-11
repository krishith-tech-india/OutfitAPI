using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class CoreDependencies
    {
        public static void InjectDBContextDependencies(this IServiceCollection service, string connString)
        {
            //service.AddDbContext<DBContext>(options => options.UseSqlServer(connString));
        }
    }
}
