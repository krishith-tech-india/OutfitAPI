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
            service.AddScoped<IOrderStatusService, OrderStatusService>();
            service.AddScoped<IProductCategoryService, ProductCategoryService>();
            service.AddScoped <IProductGroupService, ProductGroupService>();
            service.AddScoped <IProductService, ProductService>();
            service.AddScoped <IReviewService, ReviewService>();
            service.AddScoped <ICartService, CartService>();
        }
    }
}
