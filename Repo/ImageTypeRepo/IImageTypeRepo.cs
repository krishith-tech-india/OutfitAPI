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
    Task<List<ImageType>> GetAllImageTypeAsync(PaginationDto paginationDto);
    Task<ImageType> GetImageTypeByIdAsync(int id);
    Task<bool> CheckIsImageTypeExistByNameAsync(string name);
    Task InsertImageTypeAsync(ImageType imageType);
    Task UpdateImageTypeAsync(ImageType imageType);
}
