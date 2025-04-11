using Microsoft.Extensions.DependencyInjection;

namespace Service
{
    public static class ServiceDependencies
    {
        public static void InjectServiceDependencies(this IServiceCollection service)
        {
            //service.AddScoped<IUserService, UserService>();

        }
    }
}
