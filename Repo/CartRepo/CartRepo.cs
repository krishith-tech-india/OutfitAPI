using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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
        CheckIsCartDataValidOrNotAsync(cart);
        var cartID = await GetQueyable().FirstOrDefaultAsync(x => x.ProductId.Equals(cart.ProductId) && x.UserId.Equals(cart.UserId) && !x.IsDeleted);
        if (cartID != null)
        {
            cartID.Quantity += cart.Quantity;
            await UpdateCartAsync(cartID);
        }
        else
        {
            cart.AddedOn = DateTime.Now;
            cart.AddedBy = _userContext.loggedInUser.Id;
            await InsertAsync(cart);
            await SaveChangesAsync();
        }
    }

    public async Task UpdateCartAsync(Cart cart)
    {
        CheckIsCartDataValidOrNotAsync(cart);
        cart.LastUpdatedOn = DateTime.Now;
        cart.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(cart);
        await SaveChangesAsync();
    }

    private void CheckIsCartDataValidOrNotAsync(Cart cart)
    {
        if (cart.Quantity <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Cart ", "Quantity"));
    }
}
