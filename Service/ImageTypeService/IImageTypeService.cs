using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IImageTypeService
{
    Task<List<ImageTypeDto>> GetImageTypeAsync();
    Task<ImageTypeDto> GetImageTypeByIdAsync(int id);
    Task AddImageType(ImageTypeDto imageTypeDto);
    Task UpadateImageTypeAsync(int id, ImageTypeDto imageTypeDto);
    Task DeleteImageTypeAsync(int id);
}
