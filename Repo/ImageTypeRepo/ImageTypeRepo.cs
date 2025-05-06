using Core;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class ImageTypeRepo : BaseRepo<ImageType>, IImageTypeRepo
{
    public ImageTypeRepo(OutfitDBContext context) : base(context)
    {

    }

    public async Task CheckDataValidOrnotAsync(ImageType imageType)
    {
        if (string.IsNullOrWhiteSpace(imageType.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Image Type name is required");
        if (string.IsNullOrWhiteSpace(imageType.Description))
            imageType.Description = null;
    }

    public async Task<List<ImageType>> GetAllImageTypeAsync()
    {
        List<ImageType> imageTypes = await Select(x => !x.IsDeleted).ToListAsync();
        if (imageTypes.Count == 0)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Image Type Data Not exist");
        return imageTypes;
    }

    public async Task<ImageType> GetImageTypeByIdAsync(int id)
    {
        var imageTypes = await GetByIdAsync(id);
        if (imageTypes == null || imageTypes.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Image Types id {id} not exist");
        return imageTypes;
    }

    public async Task InsertImageTypeAsync(ImageType imageType)
    {
        await CheckDataValidOrnotAsync(imageType);
        imageType.AddedOn = DateTime.Now;
        //imageType.AddedBy = 0;
        await InsertAsync(imageType);
        await SaveChangesAsync();
    }

    public async Task UpdateImageTypeAsync(ImageType imageType)
    {
        await CheckDataValidOrnotAsync(imageType);
        imageType.LastUpdatedOn = DateTime.Now;
        //user.LastUpdatedBy = 0;
        Update(imageType);
        await SaveChangesAsync();
    }

}
