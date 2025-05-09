using Microsoft.Extensions.DependencyInjection;

namespace Mapper
{
    public static class MapperDependencies
    {
        public static void InjectMapperDependnecies(this IServiceCollection service)
        {
            service.AddScoped<IRoleMapper, RoleMapper>();
            service.AddScoped<IUserMapper, UserMapper>();
            service.AddScoped<IAddressMapper, AddressMapper>();
            service.AddScoped<IImageTypeMapper, ImageTypeMapper>();
            service.AddScoped<IOrderStatusMapper, OrderStatusMapper>();
        }
    }
}
