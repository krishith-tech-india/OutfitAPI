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

    public async Task<List<ImageTypeDto>> GetImageTypesAsync(ImageTypeFilterDto imageTypeFilterDto)
    {
        IQueryable<ImageType> imageTypeQuery = _imageTypeRepo.GetQueyable();
        var imageTypeFilter = PradicateBuilder.True<ImageType>().And(x => !x.IsDeleted);

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.GenericTextFilter))
            imageTypeFilter = imageTypeFilter.And(x =>
                        x.Name.ToLower().Contains(imageTypeFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(imageTypeFilterDto.GenericTextFilter.ToLower()))
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.NameFilterText))
            imageTypeFilter = imageTypeFilter.And(x => x.Name.ToLower().Contains(imageTypeFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.DescriptionFilterText))
            imageTypeFilter = imageTypeFilter.And(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(imageTypeFilterDto.DescriptionFilterText.ToLower()));

        imageTypeQuery = imageTypeQuery.Where(imageTypeFilter);

        //OrderByQuery

        Expression<Func<ImageType, object>> OrderByExpression;

        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.OrderByField) && imageTypeFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            OrderByExpression = imageType => imageType.Name ?? "";
        else if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.OrderByField) && imageTypeFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            OrderByExpression = imageType => imageType.Description ?? "";
        else
            OrderByExpression = imageType => imageType.Id;

        imageTypeQuery = 
            imageTypeFilterDto.OrderByEnumValue == null || imageTypeFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Asc)
            ? imageTypeQuery.OrderBy(OrderByExpression)
            : imageTypeQuery.OrderByDescending(OrderByExpression);

        //Pagination
        if (imageTypeFilterDto.IsPagination)
            imageTypeQuery = imageTypeQuery.Skip((imageTypeFilterDto.PageNo - 1) * imageTypeFilterDto.PageSize).Take(imageTypeFilterDto.PageSize);

        return await imageTypeQuery.Select(x => _imageTypeMapper.GetImageTypeDto(x)).ToListAsync();
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

    public async Task UpadateImageTypeAsync(int id,ImageTypeDto imageTypeDto)
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
}
