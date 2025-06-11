using Core;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Service;

public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    private readonly IProductMapper _productMapper;
    private readonly IProductGroupRepo _productGroupRepo;
    private readonly IImageRepo _imageRepo;
    private readonly IImageTypeRepo _imageTypeRepo;

    public ProductService(
        IProductRepo productRepo,
        IProductMapper productMapper,
        IProductGroupRepo productGroupRepo,
        IImageRepo imageRepo,
        IImageTypeRepo imageTypeRepo

    )
    {
        _productGroupRepo = productGroupRepo;
        _productRepo = productRepo;
        _productMapper = productMapper;
        _imageRepo = imageRepo;
        _imageTypeRepo = imageTypeRepo;
    }

    public async Task<List<ProductDto>> GetProductsAsync(ProductFilterDto productFilterDto)
    {
        var productQueyable = _productRepo.GetQueyable();
        var productGroupQueyable = _productGroupRepo.GetQueyable();

        IQueryable<ProductDto> productQuery = productQueyable
            .Join(
                productGroupQueyable,
                product => product.ProductGroupId,
                productgroup => productgroup.Id,
                (product, productgroup) => new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.OriginalPrice,
                    product.DiscountedPrice,
                    product.SubTitle,
                    product.Strach,
                    product.Softness,
                    product.Transparency,
                    product.Fabric,
                    product.Color,
                    product.Print,
                    product.Size,
                    product.Features,
                    product.Length,
                    product.DeliveryPrice,
                    product.WashingInstruction,
                    product.IroningInstruction,
                    product.BleachingInstruction,
                    product.DryingInstruction,
                    product.ProductGroupId,
                    productGroupName = productgroup.Name,
                    productGroupDeleted = productgroup.IsDeleted,
                    product.IsDeleted
                }
            )
            .Where(x => !x.IsDeleted && !x.productGroupDeleted)
            .Select(x => new ProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OriginalPrice = x.OriginalPrice.Value,
                DiscountedPrice = x.DiscountedPrice.Value,
                SubTitle = x.SubTitle,
                Strach = x.Strach,
                Softness = x.Softness,
                Transparency = x.Transparency,
                Fabric = x.Fabric,
                Color = x.Color,
                Print = x.Print,
                Size = x.Size.Value,
                Features = x.Features,
                Length = x.Length,
                DeliveryPrice = x.DeliveryPrice.Value,
                WashingInstruction = x.WashingInstruction,
                IroningInstruction = x.IroningInstruction,
                BleachingInstruction = x.BleachingInstruction,
                DryingInstruction = x.DryingInstruction,
                ProductGroupId = x.ProductGroupId.Value,
                ProductGroupName = x.productGroupName
            });

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productFilterDto.GenericTextFilter))
            productQuery = productQuery.Where(x =>
                        x.Name.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.OriginalPrice.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        x.DiscountedPrice.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.Strach.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        x.Softness.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        x.Transparency.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Fabric) && x.Fabric.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Color) && x.Color.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Print) && x.Print.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.Size.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Length) && x.Length.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.DeliveryPrice.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.WashingInstruction) && x.WashingInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.IroningInstruction) && x.IroningInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.BleachingInstruction) && x.BleachingInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.DryingInstruction) && x.DryingInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.ProductGroupName.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())
                    );

        // FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productFilterDto.NameFilterText))
            productQuery = productQuery.Where(x => x.Name.ToLower().Contains(productFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DescriptionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productFilterDto.DescriptionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.OriginalPriceFilterText))
            productQuery = productQuery.Where(x => x.OriginalPrice.ToString().Contains(productFilterDto.OriginalPriceFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DiscountedPriceFilterText))
            productQuery = productQuery.Where(x => x.DiscountedPrice.ToString().Contains(productFilterDto.DiscountedPriceFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.SubTitleFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productFilterDto.SubTitleFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.StrachFilterText))
            productQuery = productQuery.Where(x => x.Strach.HasValue && x.Strach.Value.ToString().Contains(productFilterDto.StrachFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.SoftnessFilterText))
            productQuery = productQuery.Where(x => x.Softness.HasValue && x.Softness.Value.ToString().Contains(productFilterDto.SoftnessFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.TransparencyFilterText))
            productQuery = productQuery.Where(x => x.Transparency.HasValue && x.Transparency.Value.ToString().Contains(productFilterDto.TransparencyFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.FabricFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Fabric) && x.Fabric.ToLower().Contains(productFilterDto.FabricFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.ColorFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Color) && x.Color.ToLower().Contains(productFilterDto.ColorFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.PrintFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Print) && x.Print.ToLower().Contains(productFilterDto.PrintFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.SizeFilterText))
            productQuery = productQuery.Where(x => x.Size.ToString().Contains(productFilterDto.SizeFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.FeaturesFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productFilterDto.FeaturesFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.LengthFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Length) && x.Length.ToLower().Contains(productFilterDto.LengthFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DeliveryPriceFilterText))
            productQuery = productQuery.Where(x => x.DeliveryPrice.ToString().Contains(productFilterDto.DeliveryPriceFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.WashingInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.WashingInstruction) && x.WashingInstruction.ToLower().Contains(productFilterDto.WashingInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.BleachingInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.BleachingInstruction) && x.BleachingInstruction.ToLower().Contains(productFilterDto.BleachingInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.IroningInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.IroningInstruction) && x.IroningInstruction.ToLower().Contains(productFilterDto.IroningInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DryingInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.DryingInstruction) && x.DryingInstruction.ToLower().Contains(productFilterDto.DryingInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.ProductGroupNameFilterText))
            productQuery = productQuery.Where(x => x.ProductGroupName.ToLower().Contains(productFilterDto.ProductGroupNameFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Description);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByOriginalPriceValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.OriginalPrice);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDiscountedPriceValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.DiscountedPrice);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySubTitleValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.SubTitle);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByStrachValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Strach);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySoftnessValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Softness);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByTransparencyValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Transparency);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByFabricValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Fabric);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByColorValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Color);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByPrintValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Print);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySizeValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Size);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByFeaturesValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Features);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLengthValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Length);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDeliveryPriceValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.DeliveryPrice);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByWashingInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.WashingInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByIroningInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.IroningInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByBleachingInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.BleachingInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDryingInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.DryingInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByGroupIdValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.ProductGroupId);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByGroupNameValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.ProductGroupName);
        else
            productQuery = productQuery.OrderBy(x => x.Id);

        //Pagination
        if (productFilterDto.IsPagination)
            productQuery = productQuery.Skip((productFilterDto.PageNo - 1) * productFilterDto.PageSize).Take(productFilterDto.PageSize);

        return await productQuery.ToListAsync();
    }
    public async Task<ProductDto> GetProductByIDAsync(int id)
    {
        var productQueyable = _productRepo.GetQueyable();
        var productGroupQueyable = _productGroupRepo.GetQueyable();

        var productQuery = await productQueyable
            .Join(
                productGroupQueyable,
                product => product.ProductGroupId,
                productgroup => productgroup.Id,
                (product, productgroup) => new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.OriginalPrice,
                    product.DiscountedPrice,
                    product.SubTitle,
                    product.Strach,
                    product.Softness,
                    product.Transparency,
                    product.Fabric,
                    product.Color,
                    product.Print,
                    product.Size,
                    product.Features,
                    product.Length,
                    product.DeliveryPrice,
                    product.WashingInstruction,
                    product.IroningInstruction,
                    product.BleachingInstruction,
                    product.DryingInstruction,
                    product.ProductGroupId,
                    productGroupName = productgroup.Name,
                    productGroupDeleted = productgroup.IsDeleted,
                    product.IsDeleted
                }
            )
            .Where(x => x.Id == id && !x.IsDeleted && !x.productGroupDeleted)
            .Select(x => new ProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OriginalPrice = x.OriginalPrice.Value,
                DiscountedPrice = x.DiscountedPrice.Value,
                SubTitle = x.SubTitle,
                Strach = x.Strach,
                Softness = x.Softness,
                Transparency = x.Transparency,
                Fabric = x.Fabric,
                Color = x.Color,
                Print = x.Print,
                Size = x.Size.Value,
                Features = x.Features,
                Length = x.Length,
                DeliveryPrice = x.DeliveryPrice.Value,
                WashingInstruction = x.WashingInstruction,
                IroningInstruction = x.IroningInstruction,
                BleachingInstruction = x.BleachingInstruction,
                DryingInstruction = x.DryingInstruction,
                ProductGroupId = x.ProductGroupId.Value,
                ProductGroupName = x.productGroupName
            }).FirstOrDefaultAsync();
        if (productQuery == null)
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Product", "Id", id));
        return productQuery;
    }

    public async Task<List<ProductDto>> GetProductByGroupIdAsync(int id, ProductFilterDto productFilterDto)
    {
        if (!await _productGroupRepo.IsProductGroupIdExistAsync(id))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Product", " Group Id", id));

        var productQueyable = _productRepo.GetQueyable();
        var productGroupQueyable = _productGroupRepo.GetQueyable();

        IQueryable<ProductDto> productQuery = productQueyable
            .Join(
                productGroupQueyable,
                product => product.ProductGroupId,
                productgroup => productgroup.Id,
                (product, productgroup) => new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.OriginalPrice,
                    product.DiscountedPrice,
                    product.SubTitle,
                    product.Strach,
                    product.Softness,
                    product.Transparency,
                    product.Fabric,
                    product.Color,
                    product.Print,
                    product.Size,
                    product.Features,
                    product.Length,
                    product.DeliveryPrice,
                    product.WashingInstruction,
                    product.IroningInstruction,
                    product.BleachingInstruction,
                    product.DryingInstruction,
                    product.ProductGroupId,
                    productGroupName = productgroup.Name,
                    productGroupDeleted = productgroup.IsDeleted,
                    product.IsDeleted
                }
            )
            .Where(x => x.ProductGroupId == id && !x.IsDeleted && !x.productGroupDeleted)
            .Select(x => new ProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OriginalPrice = x.OriginalPrice.Value,
                DiscountedPrice = x.DiscountedPrice.Value,
                SubTitle = x.SubTitle,
                Strach = x.Strach,
                Softness = x.Softness,
                Transparency = x.Transparency,
                Fabric = x.Fabric,
                Color = x.Color,
                Print = x.Print,
                Size = x.Size.Value,
                Features = x.Features,
                Length = x.Length,
                DeliveryPrice = x.DeliveryPrice.Value,
                WashingInstruction = x.WashingInstruction,
                IroningInstruction = x.IroningInstruction,
                BleachingInstruction = x.BleachingInstruction,
                DryingInstruction = x.DryingInstruction,
                ProductGroupId = x.ProductGroupId.Value,
                ProductGroupName = x.productGroupName
            });

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productFilterDto.GenericTextFilter))
            productQuery = productQuery.Where(x =>
                        x.Name.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.OriginalPrice.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        x.DiscountedPrice.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.Strach.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        x.Softness.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        x.Transparency.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Fabric) && x.Fabric.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Color) && x.Color.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Print) && x.Print.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.Size.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Length) && x.Length.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.DeliveryPrice.ToString().ToLower().Contains(productFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.WashingInstruction) && x.WashingInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.IroningInstruction) && x.IroningInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.BleachingInstruction) && x.BleachingInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.DryingInstruction) && x.DryingInstruction.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())) ||
                        x.ProductGroupName.ToLower().Contains(productFilterDto.GenericTextFilter.ToLower())
                    );

        // FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productFilterDto.NameFilterText))
            productQuery = productQuery.Where(x => x.Name.ToLower().Contains(productFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DescriptionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productFilterDto.DescriptionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.OriginalPriceFilterText))
            productQuery = productQuery.Where(x => x.OriginalPrice.ToString().Contains(productFilterDto.OriginalPriceFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DiscountedPriceFilterText))
            productQuery = productQuery.Where(x => x.DiscountedPrice.ToString().Contains(productFilterDto.DiscountedPriceFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.SubTitleFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productFilterDto.SubTitleFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.StrachFilterText))
            productQuery = productQuery.Where(x => x.Strach.HasValue && x.Strach.Value.ToString().Contains(productFilterDto.StrachFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.SoftnessFilterText))
            productQuery = productQuery.Where(x => x.Softness.HasValue && x.Softness.Value.ToString().Contains(productFilterDto.SoftnessFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.TransparencyFilterText))
            productQuery = productQuery.Where(x => x.Transparency.HasValue && x.Transparency.Value.ToString().Contains(productFilterDto.TransparencyFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.FabricFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Fabric) && x.Fabric.ToLower().Contains(productFilterDto.FabricFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.ColorFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Color) && x.Color.ToLower().Contains(productFilterDto.ColorFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.PrintFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Print) && x.Print.ToLower().Contains(productFilterDto.PrintFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.SizeFilterText))
            productQuery = productQuery.Where(x => x.Size.ToString().Contains(productFilterDto.SizeFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.FeaturesFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productFilterDto.FeaturesFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.LengthFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.Length) && x.Length.ToLower().Contains(productFilterDto.LengthFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DeliveryPriceFilterText))
            productQuery = productQuery.Where(x => x.DeliveryPrice.ToString().Contains(productFilterDto.DeliveryPriceFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.WashingInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.WashingInstruction) && x.WashingInstruction.ToLower().Contains(productFilterDto.WashingInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.BleachingInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.BleachingInstruction) && x.BleachingInstruction.ToLower().Contains(productFilterDto.BleachingInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.IroningInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.IroningInstruction) && x.IroningInstruction.ToLower().Contains(productFilterDto.IroningInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.DryingInstructionFilterText))
            productQuery = productQuery.Where(x => !string.IsNullOrWhiteSpace(x.DryingInstruction) && x.DryingInstruction.ToLower().Contains(productFilterDto.DryingInstructionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productFilterDto.ProductGroupNameFilterText))
            productQuery = productQuery.Where(x => x.ProductGroupName.ToLower().Contains(productFilterDto.ProductGroupNameFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Description);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByOriginalPriceValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.OriginalPrice);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDiscountedPriceValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.DiscountedPrice);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySubTitleValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.SubTitle);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByStrachValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Strach);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySoftnessValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Softness);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByTransparencyValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Transparency);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByFabricValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Fabric);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByColorValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Color);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByPrintValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Print);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySizeValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Size);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByFeaturesValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Features);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLengthValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.Length);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDeliveryPriceValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.DeliveryPrice);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByWashingInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.WashingInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByIroningInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.IroningInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByBleachingInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.BleachingInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDryingInstructionValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.DryingInstruction);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByGroupIdValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.ProductGroupId);
        else if (!string.IsNullOrWhiteSpace(productFilterDto.OrderByField) && productFilterDto.OrderByField.ToLower().Equals(Constants.OrderByGroupNameValue, StringComparison.OrdinalIgnoreCase))
            productQuery = productQuery.OrderBy(x => x.ProductGroupName);
        else
            productQuery = productQuery.OrderBy(x => x.Id);

        //Pagination
        if (productFilterDto.IsPagination)
            productQuery = productQuery.Skip((productFilterDto.PageNo - 1) * productFilterDto.PageSize).Take(productFilterDto.PageSize);

        return await productQuery.ToListAsync();
    }


    public async Task InsertProductAsync(ProductDto productDto)
    {
        var tras = _productRepo.BeginTransaction();

        if(!await _productGroupRepo.IsProductGroupIdExistAsync(productDto.ProductGroupId))
            throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Product", " Group Id", productDto.ProductGroupId));
        var product = _productMapper.GetEntity(productDto);
        int productId = await _productRepo.InsertProductAsync(product);

        foreach (var image in productDto.Images)
        {
            if (string.IsNullOrWhiteSpace(image.Url))
                throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Image ", "Url"));
            if (image.ImageTypeId <= 0)
                throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Image ", "ImageTypeId"));
            if (!await _imageTypeRepo.IsImageTypeExistAsync(image.ImageTypeId))
                throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Image", "Image Type Id", image.ImageTypeId));
            await _imageRepo.InsertImageAsync(_productMapper.GetEntity(image), productId);
        }

        await _productRepo.CommitTransaction(tras);
    }

    public async Task UpdateProductAsync(int id,ProductDto productDto)
    {
        if (!await _productGroupRepo.IsProductGroupIdExistAsync(productDto.ProductGroupId))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Product", " Group Id", productDto.ProductGroupId));
        var product = await _productRepo.GetProductByIDAsync(id);
        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.OriginalPrice = productDto.OriginalPrice;
        product.DiscountedPrice = productDto.DiscountedPrice;
        product.SubTitle = productDto.SubTitle;
        product.Strach = productDto.Strach;
        product.Softness = productDto.Softness;
        product.Transparency = productDto.Transparency;
        product.Fabric = productDto.Fabric;
        product.Color = productDto.Color;
        product.Print = productDto.Print;
        product.Size = productDto.Size;
        product.Features = productDto.Features;
        product.Length = productDto.Length;
        product.DeliveryPrice = productDto.DeliveryPrice;
        product.WashingInstruction = productDto.WashingInstruction;
        product.IroningInstruction = productDto.IroningInstruction;
        product.BleachingInstruction = productDto.BleachingInstruction;
        product.DryingInstruction = productDto.DryingInstruction;
        product.ProductGroupId = productDto.ProductGroupId;
        await _productRepo.UpdateProductAsync(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepo.GetProductByIDAsync(id);
        product.IsDeleted = true;
        await _productRepo.UpdateProductAsync(product);
    }
}
