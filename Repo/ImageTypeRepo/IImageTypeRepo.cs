using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IImageTypeRepo : IBaseRepo<ImageType>
{
    Task CheckDataValidOrnotAsync(ImageType imageType);
    Task<List<ImageType>> GetAllImageTypeAsync();
    Task<ImageType> GetImageTypeByIdAsync(int id);
    Task InsertImageTypeAsync(ImageType imageType);
    Task UpdateImageTypeAsync(ImageType imageType);
}
