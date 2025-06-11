using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IImageTypeRepo : IBaseRepo<ImageType>
{
    Task<ImageType> GetImageTypeByIdAsync(int id);
    Task<bool> IsImageTypeExistByNameAsync(string name);
    Task InsertImageTypeAsync(ImageType imageType);
    Task UpdateImageTypeAsync(ImageType imageType);
    Task<bool> IsImageTypeExistAsync(int id);
}
