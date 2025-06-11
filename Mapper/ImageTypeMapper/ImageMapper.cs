using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class ImageTypeMapper : IImageTypeMapper
{
    public ImageType GetEntity(ImageTypeDto imageTypeDto)
    {
        return new ImageType
        {
            Name = imageTypeDto.Name,
            Description = imageTypeDto.Description
        };
    }

    public ImageTypeDto GetImageTypeDto(ImageType imageType)
    {
        return new ImageTypeDto
        {
            Id = imageType.Id,
            Name = imageType.Name,
            Description = imageType.Description
        };
    }
}
