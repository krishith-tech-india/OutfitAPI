using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class CartRepo : BaseRepo<Cart> , ICartRepo
{
    private readonly IUserContext _userContext;
    public CartRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task<Cart> GetCartByIDAsync(int id)
    {
        var cart = await GetByIdAsync(id);
        if (cart == null || cart.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Cart ", "Id", id));
        return cart;
    }

    public async Task InsertCartAsync(Cart cart)
    {
        await CheckIsCartDataValidOrNotAsync(cart);
        //if (await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(cart.Id) && x.ProductId.Equals(cart.ProductId)) && await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(cart.Id) && x.UserId.Equals(cart.UserId)))
        //{
        //    cart.Quantity += 1;
        //    await UpdateCartAsync(cart);
        //}
        cart.AddedOn = DateTime.Now;
        cart.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(cart);
        await SaveChangesAsync();
    }

    public async Task UpdateCartAsync(Cart cart)
    {
        await CheckIsCartDataValidOrNotAsync(cart);
        cart.LastUpdatedOn = DateTime.Now;
        cart.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(cart);
        await SaveChangesAsync();
    }

    private async Task CheckIsCartDataValidOrNotAsync(Cart cart)
    {
        if (cart.Quantity <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Cart ", "Quantity"));
    }
}
