using Core;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class ProductGroupService : IProductGroupService
{
    private readonly IProductGroupRepo _productGroupRepo;
    private readonly IProductGroupMapper _productGroupMapper;
    private readonly IProductCategoryRepo _productCategoryRepo;

    public ProductGroupService(
        IProductGroupRepo productGroupRepo,
        IProductGroupMapper productGroupMapper,
        IProductCategoryRepo productCategoryRepo
    )
    {
        _productGroupRepo = productGroupRepo;
        _productGroupMapper = productGroupMapper;
        _productCategoryRepo = productCategoryRepo;
    }

    public async Task<List<ProductGroupDto>> GetProductGroupsAsync(ProductGroupFilterDto productGroupFilterDto)
    {
        var productGroupQueyable = _productGroupRepo.GetQueyable();
        var productCategoryQueyable = _productCategoryRepo.GetQueyable();

        IQueryable<ProductGroupDto> productGroupQuery = productGroupQueyable
            .Join(
                productCategoryQueyable,
                productgroup => productgroup.CategoryId,
                productcategory => productcategory.Id,
                (productgroup, productcategory) => new
                {
                    productgroup.Id,
                    productgroup.Name,
                    productgroup.Description,
                    productgroup.Features,
                    productgroup.SubTitle,
                    productgroup.CategoryId,
                    productCategoryName = productcategory.Name,
                    productCategoryDeleted = productcategory.IsDeleted,
                    productgroup.IsDeleted
                }
            )
            .Where(x => !x.IsDeleted && !x.productCategoryDeleted)
            .Select(x => new ProductGroupDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Features = x.Features,
                SubTitle = x.SubTitle,
                CategoryId = x.CategoryId.Value,
                CategoryName = x.productCategoryName
            });

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.GenericTextFilter))
            productGroupQuery = productGroupQuery.Where(x =>
                        x.Name.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower())) ||
                        x.CategoryName.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower()) 
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.NameFilterText))
            productGroupQuery = productGroupQuery.Where(x => x.Name.ToLower().Contains(productGroupFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.DescriptionFilterText))
            productGroupQuery = productGroupQuery.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productGroupFilterDto.DescriptionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.FeaturesFilterText))
            productGroupQuery = productGroupQuery.Where(x => !string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productGroupFilterDto.FeaturesFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.SubTitleFilterText))
            productGroupQuery = productGroupQuery.Where(x => !string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productGroupFilterDto.SubTitleFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.CategoryNameFilterText))
            productGroupQuery = productGroupQuery.Where(x => x.CategoryName.ToLower().Contains(productGroupFilterDto.CategoryNameFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.Description);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByFeaturesValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.Features);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySubTitleValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.SubTitle);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCategoryIdValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.CategoryId);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCategoryNameValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.CategoryName);
        else
            productGroupQuery = productGroupQuery.OrderBy(x => x.Id);

        //Pagination
        if (productGroupFilterDto.IsPagination)
            productGroupQuery = productGroupQuery.Skip((productGroupFilterDto.PageNo - 1) * productGroupFilterDto.PageSize).Take(productGroupFilterDto.PageSize);

        return await productGroupQuery.ToListAsync();
    }

    public async Task<ProductGroupDto> GetProductGroupByIDAsync(int id)
    {
        var productGroupQueyable = _productGroupRepo.GetQueyable();
        var productCategoryQueyable = _productCategoryRepo.GetQueyable();

        var productGroupQuery = await productGroupQueyable
            .Join(
                productCategoryQueyable,
                productgroup => productgroup.CategoryId,
                productcategory => productcategory.Id,
                (productgroup, productcategory) => new
                {
                    productgroup.Id,
                    productgroup.Name,
                    productgroup.Description,
                    productgroup.Features,
                    productgroup.SubTitle,
                    productgroup.CategoryId,
                    productCategoryName = productcategory.Name,
                    productCategoryDeleted = productcategory.IsDeleted,
                    productgroup.IsDeleted
                }
            )
            .Where(x => x.Id == id && !x.IsDeleted && !x.productCategoryDeleted)
            .Select(x => new ProductGroupDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Features = x.Features,
                SubTitle = x.SubTitle,
                CategoryId = x.CategoryId.Value,
                CategoryName = x.productCategoryName
            }).FirstOrDefaultAsync();

        if (productGroupQuery == null)
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Product Group", "Id", id));

        return productGroupQuery;
    }

    public async Task<List<ProductGroupDto>> GetProductGroupByCategoryIdAsync(int id , ProductGroupFilterDto productGroupFilterDto)
    {
        if (!await _productCategoryRepo.IsProductCategoryIdExistAsync(id))
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Product Group", "Category Id", id));

        var productGroupQueyable = _productGroupRepo.GetQueyable();
        var productCategoryQueyable = _productCategoryRepo.GetQueyable();

        IQueryable<ProductGroupDto> productGroupQuery = productGroupQueyable
            .Join(
                productCategoryQueyable,
                productgroup => productgroup.CategoryId,
                productcategory => productcategory.Id,
                (productgroup, productcategory) => new
                {
                    productgroup.Id,
                    productgroup.Name,
                    productgroup.Description,
                    productgroup.Features,
                    productgroup.SubTitle,
                    productgroup.CategoryId,
                    productCategoryName = productcategory.Name,
                    productCategoryDeleted = productcategory.IsDeleted,
                    productgroup.IsDeleted
                }
            )
            .Where(x => x.CategoryId == id && !x.IsDeleted && !x.productCategoryDeleted)
            .Select(x => new ProductGroupDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Features = x.Features,
                SubTitle = x.SubTitle,
                CategoryId = x.CategoryId.Value,
                CategoryName = x.productCategoryName
            });

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.GenericTextFilter))
            productGroupQuery = productGroupQuery.Where(x =>
                        x.Name.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower())) ||
                        x.CategoryName.ToLower().Contains(productGroupFilterDto.GenericTextFilter.ToLower())
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.NameFilterText))
            productGroupQuery = productGroupQuery.Where(x => x.Name.ToLower().Contains(productGroupFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.DescriptionFilterText))
            productGroupQuery = productGroupQuery.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productGroupFilterDto.DescriptionFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.FeaturesFilterText))
            productGroupQuery = productGroupQuery.Where(x => !string.IsNullOrWhiteSpace(x.Features) && x.Features.ToLower().Contains(productGroupFilterDto.FeaturesFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.SubTitleFilterText))
            productGroupQuery = productGroupQuery.Where(x => !string.IsNullOrWhiteSpace(x.SubTitle) && x.SubTitle.ToLower().Contains(productGroupFilterDto.SubTitleFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.CategoryNameFilterText))
            productGroupQuery = productGroupQuery.Where(x => x.CategoryName.ToLower().Contains(productGroupFilterDto.CategoryNameFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.Description);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByFeaturesValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.Features);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderBySubTitleValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.SubTitle);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCategoryIdValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.CategoryId);
        else if (!string.IsNullOrWhiteSpace(productGroupFilterDto.OrderByField) && productGroupFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCategoryNameValue, StringComparison.OrdinalIgnoreCase))
            productGroupQuery = productGroupQuery.OrderBy(x => x.CategoryName);
        else
            productGroupQuery = productGroupQuery.OrderBy(x => x.Id);

        //Pagination
        if (productGroupFilterDto.IsPagination)
            productGroupQuery = productGroupQuery.Skip((productGroupFilterDto.PageNo - 1) * productGroupFilterDto.PageSize).Take(productGroupFilterDto.PageSize);
        return await productGroupQuery.ToListAsync();
    }

    public async Task InsertProductGroupAsync(ProductGroupDto productGroupDto)
    {
        if(!await _productCategoryRepo.IsProductCategoryIdExistAsync(productGroupDto.CategoryId))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Product Group", "Category Id", productGroupDto.CategoryId));
        var productGroup = _productGroupMapper.GetEntity(productGroupDto);
        await _productGroupRepo.InsertProductGroupAsync(productGroup);
    }

    public async Task UpdateProductGroupAsync(int id, ProductGroupDto productGroupDto)
    {
        if (!await _productCategoryRepo.IsProductCategoryIdExistAsync(productGroupDto.CategoryId))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Product Group", "Category Id", productGroupDto.CategoryId));
        var productGroup = await _productGroupRepo.GetProductGroupByIDAsync(id);
        productGroup.Name = productGroupDto.Name;
        productGroup.Description = productGroupDto.Description;
        productGroup.CategoryId = productGroupDto.CategoryId;
        productGroup.SubTitle = productGroupDto.SubTitle;
        productGroup.Features = productGroupDto.Features;
        await _productGroupRepo.UpdateProductGroupAsync(productGroup);
    }

    public async Task DeleteProductGroupAsync(int id)
    {
        var productGroup = await _productGroupRepo.GetProductGroupByIDAsync(id);
        productGroup.IsDeleted = true;
        await _productGroupRepo.UpdateProductGroupAsync(productGroup);
    }

    public async Task<bool> IsProductGroupExistByName(string name)
    {
        return await _productGroupRepo.IsProductGroupExistByName(name);
    }
}
