﻿using Data.Models;
using Dto;
using Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IImageTypeService
{
    Task<PaginatedList<ImageTypeDto>> GetImageTypesAsync(ImageTypeFilterDto imageTypeFilterDto);
    Task<ImageTypeDto> GetImageTypeByIdAsync(int id);
    Task<bool> IsImageTypeExistByNameAsync(string name);
    Task InsertImageTypeAsync(ImageTypeDto imageTypeDto);
    Task UpadateImageTypeAsync(int id, ImageTypeDto imageTypeDto);
    Task DeleteImageTypeAsync(int id);
}
