using Core;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepo;
        private readonly ICartMapper _cartMapper;
        private readonly IUserRepo _userRepo;
        private readonly IProductRepo _productRepo;

        public CartService(
            ICartRepo cartRepo,
            ICartMapper cartMapper,
            IUserRepo userRepo,
            IProductRepo productRepo
        )
        {
            _cartRepo = cartRepo;
            _cartMapper = cartMapper;
            _userRepo = userRepo;
            _productRepo = productRepo;
        }

        public async Task<List<CartDto>> GetCartsAsync(CartFilterDto cartFilterDto)
        {
            var CartQueyable = _cartRepo.GetQueyable();
            var userQueyable = _userRepo.GetQueyable();
            var productQueyable = _productRepo.GetQueyable();

            IQueryable<CartDto> cartQuery = CartQueyable
            .Join(
                userQueyable,
                cart => cart.UserId,
                user => user.Id,
            (cart, user) => new { cart, user })
            .Join(
                productQueyable,
                cart => cart.cart.ProductId,
                product => product.Id,
            (cart, product) => new
                {
                    cart.cart.Id,
                    cart.cart.ProductId,
                    productName = product.Name,
                    cart.cart.UserId,
                    userName = cart.user.Name,
                    cart.cart.Quantity,
                    cart.cart.IsDeleted,
                    userDeleted = cart.user.IsDeleted,
                    productDeleted = product.IsDeleted,
                }
            )
            .Where(x => !x.IsDeleted && !x.productDeleted)
            .Select(x => new CartDto()
            {
                Id = x.Id,
                ProductID = x.ProductId.Value,
                ProductName = x.productName,
                UserID = x.UserId.Value,
                UserName = x.userName,
                Quantity = x.Quantity.Value,
            });

            return await cartQuery.ToListAsync();
        }

        public async Task InsertCartAsync(CartDto cartDto)
        {
            if (!await _userRepo.IsUserIdExistAsync(cartDto.UserID))
                throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Cart ", "User Id", cartDto.UserID));
            if (!await _productRepo.IsProductIdExistAsync(cartDto.ProductID))
                throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Cart ", "Product Id", cartDto.ProductID));
            var cart = _cartMapper.GetEntity(cartDto);
            await _cartRepo.InsertCartAsync(cart);
        }

        public async Task UpdateCartAsync(int id,CartDto cartDto)
        {
            if (!await _userRepo.IsUserIdExistAsync(cartDto.UserID))
                throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Cart ", "User Id", cartDto.UserID));
            if (!await _productRepo.IsProductIdExistAsync(cartDto.ProductID))
                throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Cart ", "Product Id", cartDto.ProductID));
            var cart = await _cartRepo.GetCartByIDAsync(id);
            cart.UserId = cartDto.UserID;
            cart.ProductId = cartDto.ProductID;
            cart.Quantity = cartDto.Quantity;
            await _cartRepo.UpdateCartAsync(cart);
        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = await _cartRepo.GetCartByIDAsync(id);
            cart.IsDeleted = true;
            await _cartRepo.UpdateCartAsync(cart);
        }
    }
}
