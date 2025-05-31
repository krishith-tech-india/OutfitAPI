using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class ProductCategoryRepo : BaseRepo<ProductCategory>, IProductCategoryRepo
{
    private readonly IUserContext _userContext;
    public ProductCategoryRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }
    
    public async Task<ProductCategory> GetProductCategoryByIdAsync(int id)
    {
        var productCategory = await GetByIdAsync(id);
        if (productCategory == null || productCategory.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Product Category", "Id", id));
        return productCategory;
    }

    public async Task InsertProductCategoryAsync(ProductCategory productCategory)
    {
        await IsProductCategoryDataValidAsync(productCategory);
        if (string.IsNullOrWhiteSpace(productCategory.Description))
            productCategory.Description = null;
        productCategory.AddedOn = DateTime.Now;
        productCategory.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(productCategory);
        await SaveChangesAsync();
    }

    public async Task UpadteProductCategoryAsync(ProductCategory productCategory)
    {
        await IsProductCategoryDataValidAsync(productCategory);
        if (string.IsNullOrWhiteSpace(productCategory.Description))
            productCategory.Description = null;
        productCategory.LastUpdatedOn = DateTime.Now;
        productCategory.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(productCategory);
        await SaveChangesAsync();
    }

    public async Task<bool> IsProductCategoryExistByNameAsync(string Name)
    {
        return await AnyAsync(x => !x.IsDeleted && x.Name.ToLower().Equals(Name.ToLower()));
    }


    public async Task<bool> IsProductCategoryIdExistAsync(int id)
    {
        return await AnyAsync(x => x.Id.Equals(id) && !x.IsDeleted);
    }

    private async Task IsProductCategoryDataValidAsync(ProductCategory productCategory)
    {
        if (string.IsNullOrWhiteSpace(productCategory.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product Category", "Name"));
        if (await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(productCategory.Id) && x.Name.ToLower().Equals(productCategory.Name.ToLower())))
            throw new ApiException(System.Net.HttpStatusCode.Conflict, string.Format(Constants.AleadyExistExceptionMessage, "Product Category", "Name", productCategory.Name));
    }
}
