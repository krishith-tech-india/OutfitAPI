using Microsoft.Extensions.DependencyInjection;

namespace Repo
{
    public static class RepoDependencies
    {
        public static void InjectRepoDependencies(this IServiceCollection service)
        {
            service.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));
            service.AddScoped<IRoleRepo, RoleRepo>();
            service.AddScoped<IUserRepo, UserRepo>();
            service.AddScoped<IAddressRepo, AddressRepo>();
            service.AddScoped<IImageTypeRepo, ImageTypeRepo>();
            service.AddScoped<IOrderStatusRepo, OrderStatusRepo>();
            service.AddScoped<IProductCategoryRepo, ProductCategoryRepo>();
            service.AddScoped<IProductGroupRepo, ProductGroupRepo>();
            service.AddScoped<IProductRepo, ProductRepo>();
            service.AddScoped<IReviewRepo, ReviewRepo>();
            service.AddScoped<ICartRepo, CartRepo>();
            service.AddScoped<IImageRepo, ImageRepo>();
        }
    }
}
