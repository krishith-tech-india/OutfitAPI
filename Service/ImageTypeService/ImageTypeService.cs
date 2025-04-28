using Data.Models;
using Dto;
using Mapper;
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

    public async Task<List<ImageTypeDto>> GetImageTypeAsync()
    {
        var imageType = await _imageTypeRepo.GetAllImageTypeAsync();
        return imageType.Select(x => _imageTypeMapper.GetImageTypeDto(x)).ToList();
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
