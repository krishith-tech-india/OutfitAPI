using Core;
using Data.Models;
using Dto;
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

    public async Task<List<ProductCategoryDto>> GetProductCategorysAsync(ProductCategoryFilterDto productCategoryFilterDto)
    {

        IQueryable<ProductCategory> productCategoryQuery = _productCategoryRepo.GetQueyable();
        var productCategoryFilter = PradicateBuilder.True<ProductCategory>().And(x => !x.IsDeleted);

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.GenericTextFilter))
            productCategoryFilter = productCategoryFilter.And(x =>
                        x.Name.ToLower().Contains(productCategoryFilterDto.GenericTextFilter.ToLower())||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productCategoryFilterDto.GenericTextFilter.ToLower()))
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.NameFilterText))
            productCategoryFilter = productCategoryFilter.And(x => x.Name.ToLower().Contains(productCategoryFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.DescriptionFilterText))
            productCategoryFilter = productCategoryFilter.And(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(productCategoryFilterDto.DescriptionFilterText.ToLower()));

        productCategoryQuery = productCategoryQuery.Where(productCategoryFilter);

        //OrderByQuery

        Expression<Func<ProductCategory, object>> OrderByExpression;

        if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.OrderByField) && productCategoryFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            OrderByExpression = productCategory => productCategory.Name ?? "";
        else if (!string.IsNullOrWhiteSpace(productCategoryFilterDto.OrderByField) && productCategoryFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            OrderByExpression = productCategory => productCategory.Description ?? "";
        else
            OrderByExpression = productCategory => productCategory.Id;

        productCategoryQuery =
            productCategoryFilterDto.OrderByEnumValue == null || productCategoryFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Asc)
            ? productCategoryQuery.OrderBy(OrderByExpression)
            : productCategoryQuery.OrderByDescending(OrderByExpression);

        //Pagination
        if (productCategoryFilterDto.IsPagination)
            productCategoryQuery = productCategoryQuery.Skip((productCategoryFilterDto.PageNo - 1) * productCategoryFilterDto.PageSize).Take(productCategoryFilterDto.PageSize);

        return await productCategoryQuery.Select(x => _productCategoryMapper.GetProductCategoryDto(x)).ToListAsync();
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
}
