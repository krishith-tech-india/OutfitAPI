using Core;
using Data.Models;
using Dto;
using Dto.Common;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public async Task<PaginatedList<ProductGroupDto>> GetProductGroupsAsync(ProductGroupFilterDto productGroupFilterDto)
    {
        // create paginated Product Groups List
        var paginatedProductGroupsList = new PaginatedList<ProductGroupDto>();

        //create Predicates
        var productGroupsFilterPredicate = PradicateBuilder.True<ProductGroup>();
        var productCategoryFilterPredicate = PradicateBuilder.True<ProductCategory>();

        //Apply Product Groups is Deleted filter
        productGroupsFilterPredicate = productGroupsFilterPredicate.And(x => !x.IsDeleted);

        //Apply Product Category is Deleted filter
        productCategoryFilterPredicate = productCategoryFilterPredicate.And(x => !x.IsDeleted);

        //Get address filters
        productGroupsFilterPredicate = ApplyProductGroupsFilters(productGroupsFilterPredicate, productGroupFilterDto);

        //Get user filters
        productCategoryFilterPredicate = ApplyProductCategoryFilters(productCategoryFilterPredicate, productGroupFilterDto);

        //Apply filters
        var productGroupQueyable = _productGroupRepo.GetQueyable().Where(productGroupsFilterPredicate);
        var productCategoryQueyable = _productCategoryRepo.GetQueyable().Where(productCategoryFilterPredicate);

        //join
        IQueryable<ProductGroupDto> productGroupQuery = productGroupQueyable
            .Join(
                productCategoryQueyable,
                productgroup => productgroup.CategoryId,
                productcategory => productcategory.Id,
                (productgroup, productcategory) => new ProductGroupDto()
                {
                    Id = productgroup.Id,
                    Name = productgroup.Name,
                    Description = productgroup.Description,
                    Features = productgroup.Features,
                    SubTitle = productgroup.SubTitle,
                    CategoryId = productgroup.CategoryId.Value,
                    CategoryName = productcategory.Name
                }
            );

        //ApplyGenericFilter
        productGroupQuery = ApplyGenericFilters(productGroupQuery, productGroupFilterDto);

        //OrderBy
        productGroupQuery = ApplyOrderByFilter(productGroupQuery, productGroupFilterDto);

        //FatchTotalCount
        paginatedProductGroupsList.Count = await productGroupQuery.CountAsync();

        //Pagination
        productGroupQuery = ApplyPaginationFilter(productGroupQuery, productGroupFilterDto);

        //FatchItems
        paginatedProductGroupsList.Items = await productGroupQuery.ToListAsync();

        return paginatedProductGroupsList;
    }


    private IQueryable<ProductGroupDto> ApplyGenericFilters(IQueryable<ProductGroupDto> productGroupQuery, ProductGroupFilterDto productGroupFilterDto)
    {

        //Generic filters
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<ProductGroupDto>();
            var filterText = productGroupFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.Name, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Description, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Features, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.SubTitle, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.CategoryName, $"%{filterText}%"));

            //Apply generic filters
            return productGroupQuery.Where(genericFilterPredicate);
        }

        return productGroupQuery;
    }

    public async Task<ProductGroupDto> GetProductGroupByIDAsync(int id)
    {
        var productGroupQueyable = _productGroupRepo.GetQueyable().Where(x => x.Id == id && !x.IsDeleted);
        var productCategoryQueyable = _productCategoryRepo.GetQueyable().Where(x => !x.IsDeleted);

        var productGroupQuery = await productGroupQueyable
            .Join(
                productCategoryQueyable,
                productgroup => productgroup.CategoryId,
                productcategory => productcategory.Id,
                (productgroup, productcategory) => new ProductGroupDto()
                {
                    Id = productgroup.Id,
                    Name = productgroup.Name,
                    Description = productgroup.Description,
                    Features = productgroup.Features,
                    SubTitle = productgroup.SubTitle,
                    CategoryId = productgroup.CategoryId.Value,
                    CategoryName = productcategory.Name
                }
            ).FirstOrDefaultAsync();

        if (productGroupQuery == null)
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Product Group", "Id", id));

        return productGroupQuery;
    }

    public async Task<PaginatedList<ProductGroupDto>> GetProductGroupByCategoryIdAsync(int categoryid, ProductGroupFilterDto productGroupFilterDto)
    {
        if (!await _productCategoryRepo.IsProductCategoryIdExistAsync(categoryid))
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Product Group", "Category Id", categoryid));

        // create paginated Product Groups List
        var paginatedProductGroupsList = new PaginatedList<ProductGroupDto>();

        //create Predicates
        var productGroupsFilterPredicate = PradicateBuilder.True<ProductGroup>();
        var productCategoryFilterPredicate = PradicateBuilder.True<ProductCategory>();

        //Apply Product Groups Category id filter
        productGroupsFilterPredicate = productGroupsFilterPredicate.And(x => !x.IsDeleted);
        productGroupsFilterPredicate = productGroupsFilterPredicate.And(x => x.CategoryId.Equals(categoryid));
        productCategoryFilterPredicate = productCategoryFilterPredicate.And(x => !x.IsDeleted);

        //Get address filters
        productGroupsFilterPredicate = ApplyProductGroupsFilters(productGroupsFilterPredicate, productGroupFilterDto);

        //Get user filters
        productCategoryFilterPredicate = ApplyProductCategoryFilters(productCategoryFilterPredicate, productGroupFilterDto);

        //Apply filters
        var productGroupQueyable = _productGroupRepo.GetQueyable().Where(productGroupsFilterPredicate);
        var productCategoryQueyable = _productCategoryRepo.GetQueyable().Where(productCategoryFilterPredicate);

        //join
        IQueryable<ProductGroupDto> productGroupQuery = productGroupQueyable
            .Join(
                productCategoryQueyable,
                productgroup => productgroup.CategoryId,
                productcategory => productcategory.Id,
                (productgroup, productcategory) => new ProductGroupDto()
                {
                    Id = productgroup.Id,
                    Name = productgroup.Name,
                    Description = productgroup.Description,
                    Features = productgroup.Features,
                    SubTitle = productgroup.SubTitle,
                    CategoryId = productgroup.CategoryId.Value,
                    CategoryName = productcategory.Name
                }
            );

        //ApplyGenericFilter
        productGroupQuery = ApplyGenericFilters(productGroupQuery, productGroupFilterDto);

        //OrderBy
        productGroupQuery = ApplyOrderByFilter(productGroupQuery, productGroupFilterDto);

        //FatchTotalCount
        paginatedProductGroupsList.Count = await productGroupQuery.CountAsync();

        //Pagination
        productGroupQuery = ApplyPaginationFilter(productGroupQuery, productGroupFilterDto);

        //FatchItems
        paginatedProductGroupsList.Items = await productGroupQuery.ToListAsync();

        return paginatedProductGroupsList;
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

    private Expression<Func<ProductGroup, bool>> ApplyProductGroupsFilters(Expression<Func<ProductGroup, bool>> productGroupsFilterPredicate, ProductGroupFilterDto productGroupFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.NameFilterText))
            productGroupsFilterPredicate = productGroupsFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{productGroupFilterDto.NameFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.DescriptionFilterText))
            productGroupsFilterPredicate = productGroupsFilterPredicate.And(x => EF.Functions.ILike(x.Description, $"%{productGroupFilterDto.DescriptionFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.FeaturesFilterText))
            productGroupsFilterPredicate = productGroupsFilterPredicate.And(x => EF.Functions.ILike(x.Features, $"%{productGroupFilterDto.FeaturesFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.SubTitleFilterText))
            productGroupsFilterPredicate = productGroupsFilterPredicate.And(x => EF.Functions.ILike(x.SubTitle, $"%{productGroupFilterDto.SubTitleFilterText.Trim()}%"));

        return productGroupsFilterPredicate;
    }

    private Expression<Func<ProductCategory, bool>> ApplyProductCategoryFilters(Expression<Func<ProductCategory, bool>> productCategoryFilterPredicate, ProductGroupFilterDto productGroupFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(productGroupFilterDto.CategoryNameFilterText))
            productCategoryFilterPredicate = productCategoryFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{productGroupFilterDto.CategoryNameFilterText.Trim()}%"));

        return productCategoryFilterPredicate;
    }

    private IQueryable<ProductGroupDto> ApplyOrderByFilter(IQueryable<ProductGroupDto> productGroupQuery, ProductGroupFilterDto productGroupFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<ProductGroupDto, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByNameValue, x => x.Name ?? "" },
            { Constants.OrderByDescriptionValue, x => x.Description ?? "" },
            { Constants.OrderByFeaturesValue, x => x.Features ?? "" },
            { Constants.OrderBySubTitleValue, x => x.SubTitle ?? "" },
            { Constants.OrderByCategoryIdValue, x => x.CategoryId},
            { Constants.OrderByCategoryNameValue, x => x.CategoryName ?? "" }
        };

        if (!orderByMappings.TryGetValue(productGroupFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        productGroupQuery = productGroupFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? productGroupQuery.OrderByDescending(orderByExpression)
            : productGroupQuery.OrderBy(orderByExpression);

        return productGroupQuery;
    }

    private IQueryable<ProductGroupDto> ApplyPaginationFilter(IQueryable<ProductGroupDto> productGroupQuery, ProductGroupFilterDto productGroupFilterDto)
    {
        if (productGroupFilterDto.IsPagination)
            productGroupQuery = productGroupQuery.Skip((productGroupFilterDto.PageNo - 1) * productGroupFilterDto.PageSize).Take(productGroupFilterDto.PageSize);

        return productGroupQuery;
    }
}
