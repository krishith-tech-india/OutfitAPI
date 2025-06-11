using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class ImageRepo : BaseRepo<Image>, IImageRepo
{
    private readonly IUserContext _userContext;
    public ImageRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task InsertImageAsync(Image image, int productID)
    {
        image.ProductId = productID;
        image.AddedOn = DateTime.Now;
        image.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(image);
        await SaveChangesAsync();
    }
}
