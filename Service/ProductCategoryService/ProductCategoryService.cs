using Core;
using Data.Models;
using Dto;
using Dto.Common;
using Dto.OrderStatus;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepo _productCategoryRepo;
    private readonly IProductCategoryMapper _productCategoryMapper;

    public ProductCategoryService(
        IProductCategoryRepo productCategoryRepo,
        IProductCategoryMapper productCategoryMapper
    )
    {
        _productCategoryRepo = productCategoryRepo;
        _productCategoryMapper = productCategoryMapper;
    }

    public async Task<PaginatedList<ProductCategoryDto>> GetProductCategorysAsync(ProductCategoryFilterDto productCategoryFilterDto)
    {
        // create paginated Address List
        var paginatedproductCategoryList = new PaginatedList<ProductCategoryDto>();

        //create Predicates
        var productCategoryFilterPredicate = PradicateBuilder.True<ProductCategory>();

        //Apply Product Category is Deleted filter
        productCategoryFilterPredicate = productCategoryFilterPredicate.And(x => !x.IsDeleted);

        //Get Product Category filters
        productCategoryFilterPredicate = ApplyimageTypeFilters(productCategoryFilterPredicate, productCategoryFilterDto);

        //Apply filters
        IQueryable<ProductCategory> productCategoryQuery = _productCategoryRepo.GetQueyable().Where(productCategoryFilterPredicate);

        //ApplyGenericFilter
        productCategoryQuery = ApplyGenericFilters(productCategoryQuery, productCategoryFilterDto);

        //OrderBy
        productCategoryQuery = ApplyOrderByFilter(productCategoryQuery, productCategoryFilterDto);

        //FatchTotalCount
        paginatedproductCategoryList.Count = await productCategoryQuery.CountAsync();

        //Pagination
        productCategoryQuery = ApplyPaginationFilter(productCategoryQuery, productCategoryFilterDto);

        //FatchItems
        paginatedproductCategoryList.Items = await productCategoryQuery.Select(x => _productCategoryMapper.GetProductCategoryDto(x)).ToListAsync();

        return paginatedproductCategoryList;
    }

    private IQueryable<ProductCategory> ApplyGenericFilters(IQueryable<ProductCategory> productCategoryQuery, ProductCategoryFilterDto productCategoryFilterDto)
    {
        //Generic filters
        if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<ProductCategory>();
            var filterText = productCategoryFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.Name, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Description, $"%{filterText}%"));

            //Apply generic filters
            return productCategoryQuery.Where(genericFilterPredicate);
        }

        return productCategoryQuery;
    }

    public async Task<ProductCategoryDto> GetProductCategoryByIdAsync(int id)
    {
        return _productCategoryMapper.GetProductCategoryDto(await _productCategoryRepo.GetProductCategoryByIdAsync(id));
    }

    public async Task InsertProductCategoryAsync(ProductCategoryDto productCategoryDto)
    {
        var ProductCategory = _productCategoryMapper.GetEntity(productCategoryDto);
        await _productCategoryRepo.InsertProductCategoryAsync(ProductCategory);
    }

    public async Task UpdateProductCategoryAsync(int id,ProductCategoryDto productCategoryDto)
    {
        var productCategory = await _productCategoryRepo.GetProductCategoryByIdAsync(id);
        productCategory.Name = productCategoryDto.Name;
        productCategory.Description = productCategoryDto.Description;
        await _productCategoryRepo.UpadteProductCategoryAsync(productCategory);
    }

    public async Task DeleteProductCategoryAsync(int id)
    {
        var productCategory = await _productCategoryRepo.GetProductCategoryByIdAsync(id);
        productCategory.IsDeleted = true;
        await _productCategoryRepo.UpadteProductCategoryAsync(productCategory);
    }

    public async Task<bool> IsProductCategoryExistByNameAsync(string Name)
    {
        return await _productCategoryRepo.IsProductCategoryExistByNameAsync(Name);
    }

    private Expression<Func<ProductCategory, bool>> ApplyimageTypeFilters(Expression<Func<ProductCategory, bool>> productCategoryFilterPredicate, ProductCategoryFilterDto productCategoryFilterDto)
    {
        if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.NameFilterText))
            productCategoryFilterPredicate = productCategoryFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{productCategoryFilterDto.NameFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.DescriptionFilterText))
            productCategoryFilterPredicate = productCategoryFilterPredicate.And(x => EF.Functions.ILike(x.Description, $"%{productCategoryFilterDto.DescriptionFilterText.Trim()}%"));

        return productCategoryFilterPredicate;
    }

    private IQueryable<ProductCategory> ApplyOrderByFilter(IQueryable<ProductCategory> productCategoryQuery, ProductCategoryFilterDto productCategoryFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<ProductCategory, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByNameValue, x => x.Name ?? "" },
            { Constants.OrderByDescriptionValue, x => x.Description ?? "" }
        };

        if (!orderByMappings.TryGetValue(productCategoryFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        productCategoryQuery = productCategoryFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? productCategoryQuery.OrderByDescending(orderByExpression)
            : productCategoryQuery.OrderBy(orderByExpression);

        return productCategoryQuery;
    }

    private IQueryable<ProductCategory> ApplyPaginationFilter(IQueryable<ProductCategory> productCategoryQuery, ProductCategoryFilterDto productCategoryFilterDto)
    {
        if (productCategoryFilterDto.IsPagination)
            productCategoryQuery = productCategoryQuery.Skip((productCategoryFilterDto.PageNo - 1) * productCategoryFilterDto.PageSize).Take(productCategoryFilterDto.PageSize);

        return productCategoryQuery;
    }
}
