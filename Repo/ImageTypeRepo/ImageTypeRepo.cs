using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repo;

public class ImageTypeRepo : BaseRepo<ImageType>, IImageTypeRepo
{
    private readonly IUserContext _userContext;
    public ImageTypeRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task<ImageType> GetImageTypeByIdAsync(int id)
    {
        var imageTypes = await GetByIdAsync(id);
        if (imageTypes == null || imageTypes.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Image Types", "id" ,id));
        return imageTypes;
    }

    public async Task InsertImageTypeAsync(ImageType imageType)
    {
        await IsImageTypeDataValidAsync(imageType);
        if (string.IsNullOrWhiteSpace(imageType.Description))
            imageType.Description = null;
        imageType.AddedOn = DateTime.Now;
        imageType.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(imageType);
        await SaveChangesAsync();
    }

    public async Task UpdateImageTypeAsync(ImageType imageType)
    {
        await IsImageTypeDataValidAsync(imageType);
        if (string.IsNullOrWhiteSpace(imageType.Description))
            imageType.Description = null;
        imageType.LastUpdatedOn = DateTime.Now;
        imageType.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(imageType);
        await SaveChangesAsync();
    }

    public async Task<bool> IsImageTypeExistByNameAsync(string name)
    {
        return await AnyAsync(x =>  !x.IsDeleted && x.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task<bool> IsImageTypeExistAsync(int id)
    {
        return await AnyAsync(x => !x.IsDeleted && x.Id.Equals(id));
    }

    private async Task IsImageTypeDataValidAsync(ImageType imageType)
    {
        if (string.IsNullOrWhiteSpace(imageType.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Image Type", "Name"));
        if(await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(imageType.Id) && x.Name.ToLower().Equals(imageType.Name.ToLower())))
            throw new ApiException(System.Net.HttpStatusCode.Conflict,string.Format(Constants.AleadyExistExceptionMessage, "Image Type" , "Name" , imageType.Name));
    }
}
