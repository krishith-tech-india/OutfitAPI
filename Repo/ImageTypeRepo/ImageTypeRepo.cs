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

    public async Task<List<ImageType>> GetAllImageTypeAsync(GenericFilterDto genericFilterDto)
    {
        IQueryable<ImageType> imageTypeQuery = GetQueyable().Where(x => !x.IsDeleted);

        //TextQuery
        if (!string.IsNullOrWhiteSpace(genericFilterDto.GenericTextFilter))
            imageTypeQuery = imageTypeQuery.Where(x => 
                        x.Name.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(genericFilterDto.GenericTextFilter))
                    );

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            imageTypeQuery = imageTypeQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            imageTypeQuery = imageTypeQuery.OrderBy(x => x.Description);
        else
            imageTypeQuery = imageTypeQuery.OrderBy(x => x.Id);

        //Pagination
        if (genericFilterDto.IsPagination)
            imageTypeQuery = imageTypeQuery.Skip((genericFilterDto.PageNo - 1) * genericFilterDto.PageSize).Take(genericFilterDto.PageSize);
        
        return await imageTypeQuery.ToListAsync();
    }

    public async Task<ImageType> GetImageTypeByIdAsync(int id)
    {
        var imageTypes = await GetByIdAsync(id);
        if (imageTypes == null || imageTypes.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Image Types", "id" ,id));
        return imageTypes;
    }

    public async Task<bool> CheckIsImageTypeExistByNameAsync(string name)
    {
        return await AnyAsync(x =>  !x.IsDeleted && x.Name.ToLower().Equals(name.ToLower()));
    }

    private async Task CheckIsImageTypeDataValidOrNotAsync(ImageType imageType)
    {
        if (string.IsNullOrWhiteSpace(imageType.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Image Type", "Name"));
        if(await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(imageType.Id) && x.Name.ToLower().Equals(imageType.Name.ToLower())))
            throw new ApiException(System.Net.HttpStatusCode.Conflict,string.Format(Constants.AleadyExistExceptionMessage, "Image Type" , "Name" , imageType.Name));
    }

    public async Task InsertImageTypeAsync(ImageType imageType)
    {
        await CheckIsImageTypeDataValidOrNotAsync(imageType);
        if (string.IsNullOrWhiteSpace(imageType.Description))
            imageType.Description = null;
        imageType.AddedOn = DateTime.Now;
        imageType.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(imageType);
        await SaveChangesAsync();
    }

    public async Task UpdateImageTypeAsync(ImageType imageType)
    {
        await CheckIsImageTypeDataValidOrNotAsync(imageType);
        if (string.IsNullOrWhiteSpace(imageType.Description))
            imageType.Description = null;
        imageType.LastUpdatedOn = DateTime.Now;
        imageType.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(imageType);
        await SaveChangesAsync();
    }
}
