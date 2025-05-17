using Core;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<List<ImageTypeDto>> GetImageTypeAsync(ImageTypeFilterDto imageTypeFilterDto)
    {
        IQueryable<ImageType> imageTypeQuery = _imageTypeRepo.GetQueyable().Where(x => !x.IsDeleted);

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.GenericTextFilter))
            imageTypeQuery = imageTypeQuery.Where(x =>
                        x.Name.ToLower().Contains(imageTypeFilterDto.GenericTextFilter) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(imageTypeFilterDto.GenericTextFilter))
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.NameFilterText))
            imageTypeQuery = imageTypeQuery.Where(x => x.Name.ToLower().Contains(imageTypeFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.DescriptionFilterText))
            imageTypeQuery = imageTypeQuery.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(imageTypeFilterDto.DescriptionFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.OrderByField) && imageTypeFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            imageTypeQuery = imageTypeQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(imageTypeFilterDto.OrderByField) && imageTypeFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            imageTypeQuery = imageTypeQuery.OrderBy(x => x.Description);
        else
            imageTypeQuery = imageTypeQuery.OrderBy(x => x.Id);

        //Pagination
        if (imageTypeFilterDto.IsPagination)
            imageTypeQuery = imageTypeQuery.Skip((imageTypeFilterDto.PageNo - 1) * imageTypeFilterDto.PageSize).Take(imageTypeFilterDto.PageSize);

        var imageType = await imageTypeQuery.ToListAsync();
        return imageType.Select(x => _imageTypeMapper.GetImageTypeDto(x)).ToList();
    }

    public async Task<bool> IsImageTypeExistByNameAsync(string name)
    {
        return await _imageTypeRepo.CheckIsImageTypeExistByNameAsync(name);
    }

    public async Task<ImageTypeDto> GetImageTypeByIdAsync(int id)
    {
        return _imageTypeMapper.GetImageTypeDto(await _imageTypeRepo.GetImageTypeByIdAsync(id));
    }

    public async Task AddImageType(ImageTypeDto imageTypeDto)
    {
        var ImageTypeEntity = _imageTypeMapper.GetEntity(imageTypeDto);
        await _imageTypeRepo.InsertImageTypeAsync(ImageTypeEntity);
    }

    public async Task DeleteImageTypeAsync(int id)
    {
        var imageTypes = await _imageTypeRepo.GetImageTypeByIdAsync(id);
        imageTypes.IsDeleted = true;
        await _imageTypeRepo.UpdateImageTypeAsync(imageTypes);
    }

    public async Task UpadateImageTypeAsync(int id,ImageTypeDto imageTypeDto)
    {
        var imageTypes = await _imageTypeRepo.GetImageTypeByIdAsync(id);
        imageTypes.Name = imageTypeDto.Name;
        imageTypes.Description = imageTypeDto.Description;
        await _imageTypeRepo.UpdateImageTypeAsync(imageTypes);
    }
}
