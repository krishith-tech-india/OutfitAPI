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

public class ImageTypeService : IImageTypeService
{
    private readonly IImageTypeRepo _imageTypeRepo;
    private readonly IImageTypeMapper _imageTypeMapper;
    public ImageTypeService(
        IImageTypeRepo imageTypeRepo,
        IImageTypeMapper imageTypeMapper
    )
    {
        _imageTypeRepo = imageTypeRepo;
        _imageTypeMapper = imageTypeMapper;
    }

    public async Task<PaginatedList<ImageTypeDto>> GetImageTypesAsync(ImageTypeFilterDto imageTypeFilterDto)
    {
        // create paginated Address List
        var paginatedimageTypeList = new PaginatedList<ImageTypeDto>();

        //create Predicates
        var imageTypeFilterPredicate = PradicateBuilder.True<ImageType>();

        //Apply Image Type is Deleted filter
        imageTypeFilterPredicate = imageTypeFilterPredicate.And(x => !x.IsDeleted);

        //Get Image Type filters
        imageTypeFilterPredicate = ApplyimageTypeFilters(imageTypeFilterPredicate, imageTypeFilterDto);

        //Apply filters
        IQueryable<ImageType> imageTypeQuery = _imageTypeRepo.GetQueyable().Where(imageTypeFilterPredicate);

        //ApplyGenericFilter
        imageTypeQuery = ApplyGenericFilters(imageTypeQuery, imageTypeFilterDto);

        //OrderBy
        imageTypeQuery = ApplyOrderByFilter(imageTypeQuery, imageTypeFilterDto);

        //FatchTotalCount
        paginatedimageTypeList.Count = await imageTypeQuery.CountAsync();

        //Pagination
        imageTypeQuery = ApplyPaginationFilter(imageTypeQuery, imageTypeFilterDto);

        //FatchItems
        paginatedimageTypeList.Items = await imageTypeQuery.Select(x => _imageTypeMapper.GetImageTypeDto(x)).ToListAsync();

        return paginatedimageTypeList;
    }

    private IQueryable<ImageType> ApplyGenericFilters(IQueryable<ImageType> imageTypeQuery, ImageTypeFilterDto imageTypeFilterDto)
    {
        //Generic filters
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<ImageType>();
            var filterText = imageTypeFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.Name, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Description, $"%{filterText}%"));

            //Apply generic filters
            return imageTypeQuery.Where(genericFilterPredicate);
        }

        return imageTypeQuery;
    }

    public async Task<ImageTypeDto> GetImageTypeByIdAsync(int id)
    {
        return _imageTypeMapper.GetImageTypeDto(await _imageTypeRepo.GetImageTypeByIdAsync(id));
    }

    public async Task InsertImageTypeAsync(ImageTypeDto imageTypeDto)
    {
        var ImageTypeEntity = _imageTypeMapper.GetEntity(imageTypeDto);
        await _imageTypeRepo.InsertImageTypeAsync(ImageTypeEntity);
    }

    public async Task UpadateImageTypeAsync(int id, ImageTypeDto imageTypeDto)
    {
        var imageTypes = await _imageTypeRepo.GetImageTypeByIdAsync(id);
        imageTypes.Name = imageTypeDto.Name;
        imageTypes.Description = imageTypeDto.Description;
        await _imageTypeRepo.UpdateImageTypeAsync(imageTypes);
    }

    public async Task DeleteImageTypeAsync(int id)
    {
        var imageTypes = await _imageTypeRepo.GetImageTypeByIdAsync(id);
        imageTypes.IsDeleted = true;
        await _imageTypeRepo.UpdateImageTypeAsync(imageTypes);
    }

    public async Task<bool> IsImageTypeExistByNameAsync(string name)
    {
        return await _imageTypeRepo.IsImageTypeExistByNameAsync(name);
    }

    private Expression<Func<ImageType, bool>> ApplyimageTypeFilters(Expression<Func<ImageType, bool>> imageTypeFilterPredicate, ImageTypeFilterDto imageTypeFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.NameFilterText))
            imageTypeFilterPredicate = imageTypeFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{imageTypeFilterDto.NameFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.DescriptionFilterText))
            imageTypeFilterPredicate = imageTypeFilterPredicate.And(x => EF.Functions.ILike(x.Description, $"%{imageTypeFilterDto.DescriptionFilterText.Trim()}%"));

        return imageTypeFilterPredicate;
    }

    private IQueryable<ImageType> ApplyOrderByFilter(IQueryable<ImageType> imageTypeQuery, ImageTypeFilterDto imageTypeFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<ImageType, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByNameValue, x => x.Name ?? "" },
            { Constants.OrderByDescriptionValue, x => x.Description ?? "" }
        };

        if (!orderByMappings.TryGetValue(imageTypeFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        imageTypeQuery = imageTypeFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? imageTypeQuery.OrderByDescending(orderByExpression)
            : imageTypeQuery.OrderBy(orderByExpression);

        return imageTypeQuery;
    }

    private IQueryable<ImageType> ApplyPaginationFilter(IQueryable<ImageType> imageTypeQuery, ImageTypeFilterDto imageTypeFilterDto)
    {
        if (imageTypeFilterDto.IsPagination)
            imageTypeQuery = imageTypeQuery.Skip((imageTypeFilterDto.PageNo - 1) * imageTypeFilterDto.PageSize).Take(imageTypeFilterDto.PageSize);

        return imageTypeQuery;
    }
}
