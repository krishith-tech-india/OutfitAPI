using Microsoft.Extensions.DependencyInjection;

namespace Service
{
    public static class ServiceDependencies
    {
        public static void InjectServiceDependencies(this IServiceCollection service)
        {
            service.AddScoped<IRoleService, RoleService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IAddressService, AddressService>();
            service.AddScoped<IImageTypeService, ImageTypeService>();
        }
    }
}
