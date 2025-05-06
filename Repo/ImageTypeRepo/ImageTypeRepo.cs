using Core;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class ImageTypeRepo : BaseRepo<ImageType>, IImageTypeRepo
{
    public ImageTypeRepo(OutfitDBContext context) : base(context)
    {

    }

    public void CheckDataValidOrnotAsync(ImageType imageType)
    {
        if (string.IsNullOrWhiteSpace(imageType.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Image Type name is required");
    }

    public async Task<List<ImageType>> GetAllImageTypeAsync()
    {
        return await Select(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<ImageType> GetImageTypeByIdAsync(int id)
    {
        var imageTypes = await GetByIdAsync(id);
        if (imageTypes == null || imageTypes.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Image Types id {id} not exist");
        return imageTypes;
    }

    public async Task<bool> CheckIsImageTypeExistByNameAsync(string name)
    {
        return await AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && !x.IsDeleted); ;
    }

    public async Task InsertImageTypeAsync(ImageType imageType)
    {
        CheckDataValidOrnotAsync(imageType);
        if (string.IsNullOrWhiteSpace(imageType.Description))
            imageType.Description = null;
        if (await CheckIsImageTypeExistByNameAsync(imageType.Name))
            throw new ApiException(System.Net.HttpStatusCode.Conflict, $"Image Type name {imageType.Name} aleady exist");
        imageType.AddedOn = DateTime.Now;
        //imageType.AddedBy = 0;
        await InsertAsync(imageType);
        await SaveChangesAsync();
    }

    public async Task UpdateImageTypeAsync(ImageType imageType)
    {
        CheckDataValidOrnotAsync(imageType);
        imageType.LastUpdatedOn = DateTime.Now;
        //user.LastUpdatedBy = 0;
        Update(imageType);
        await SaveChangesAsync();
    }

}
