using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class ProductRepo : BaseRepo<Product>, IProductRepo
{
    private readonly IUserContext _userContext;

    public ProductRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task<Product> GetProductByIDAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product == null || product.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Product ", "Id", id));
        return product;
    }

    public async Task<int> InsertProductAsync(Product product)
    {
        await IsProductDataValidAsync(product);
        if (string.IsNullOrWhiteSpace(product.Description))
            product.Description = null;
        if (string.IsNullOrWhiteSpace(product.SubTitle))
            product.SubTitle = null;
        if (product.Strach <= 0)
            product.Strach = 0;
        if (product.Softness <= 0)
            product.Softness = 0;
        if (product.Transparency <= 0)
            product.Transparency = 0;
        if (string.IsNullOrWhiteSpace(product.Fabric))
            product.Fabric = null;
        if (string.IsNullOrWhiteSpace(product.Print))
            product.Print = null;
        if (string.IsNullOrWhiteSpace(product.Features))
            product.Features = null;
        if (string.IsNullOrWhiteSpace(product.Length))
            product.Length = null;
        if (string.IsNullOrWhiteSpace(product.WashingInstruction))
            product.WashingInstruction = null;
        if (string.IsNullOrWhiteSpace(product.IroningInstruction))
            product.IroningInstruction = null;
        if (string.IsNullOrWhiteSpace(product.BleachingInstruction))
            product.BleachingInstruction = null;
        if (string.IsNullOrWhiteSpace(product.DryingInstruction))
            product.DryingInstruction = null;
        product.AddedOn = DateTime.Now;
        product.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(product);
        
        await SaveChangesAsync();
        return product.Id;  
    }

    public async Task UpdateProductAsync(Product product)
    {
        await IsProductDataValidAsync(product);
        if (string.IsNullOrWhiteSpace(product.Description))
            product.Description = null;
        if (string.IsNullOrWhiteSpace(product.SubTitle))
            product.SubTitle = null;
        if (product.Strach <= 0)
            product.Strach = 0;
        if (product.Softness <= 0)
            product.Softness = 0;
        if (product.Transparency <= 0)
            product.Transparency = 0;
        if (string.IsNullOrWhiteSpace(product.Fabric))
            product.Fabric = null;
        if (string.IsNullOrWhiteSpace(product.Print))
            product.Print = null;
        if (string.IsNullOrWhiteSpace(product.Features))
            product.Features = null;
        if (string.IsNullOrWhiteSpace(product.Length))
            product.Length = null;
        if (string.IsNullOrWhiteSpace(product.WashingInstruction))
            product.WashingInstruction = null;
        if (string.IsNullOrWhiteSpace(product.IroningInstruction))
            product.IroningInstruction = null;
        if (string.IsNullOrWhiteSpace(product.BleachingInstruction))
            product.BleachingInstruction = null;
        if (string.IsNullOrWhiteSpace(product.DryingInstruction))
            product.DryingInstruction = null;
        product.LastUpdatedOn = DateTime.Now;
        product.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(product);
        await SaveChangesAsync();
    }

    public async Task<bool> IsProductIdExistAsync(int id)
    {
        return await AnyAsync(x => x.Id.Equals(id) && !x.IsDeleted);
    }

    private async Task IsProductDataValidAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product ", "Name"));
        if (string.IsNullOrWhiteSpace(product.Color))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product ", "Color"));
        if (product.OriginalPrice <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product ", "Original Price"));
        if (product.DiscountedPrice <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product ", "Discounted Price"));
        if (product.Size <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product ", "Size"));
        if (product.DeliveryPrice <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product ", "Delivery Price"));
    }
}
