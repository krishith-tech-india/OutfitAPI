using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repo
{
    public class ProductGroupRepo : BaseRepo<ProductGroup>, IProductGroupRepo
    {
        private readonly IUserContext _userContext;
        public ProductGroupRepo(OutfitDBContext context, IUserContext userContext) : base(context)
        {
            _userContext = userContext;
        }

        public async Task<ProductGroup> GetProductGroupByIDAsync(int id)
        {
            var productGroup = await GetByIdAsync(id);
            if (productGroup == null || productGroup.IsDeleted)
                throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Product Group", "Id", id));
            return productGroup;
        }

        public async Task InsertProductGroupAsync(ProductGroup productGroup)
        {
            await IsProductGroupDataValidAsync(productGroup);
            if (string.IsNullOrWhiteSpace(productGroup.Description))
                productGroup.Description = null;
            if (string.IsNullOrWhiteSpace(productGroup.Features))
                productGroup.Features = null;
            if (string.IsNullOrWhiteSpace(productGroup.SubTitle))
                productGroup.SubTitle = null;
            productGroup.AddedOn = DateTime.Now;
            productGroup.AddedBy = _userContext.loggedInUser.Id;
            await InsertAsync(productGroup);
            await SaveChangesAsync();
        }

        public async Task UpdateProductGroupAsync(ProductGroup productGroup)
        {
            await IsProductGroupDataValidAsync(productGroup);
            if (string.IsNullOrWhiteSpace(productGroup.Description))
                productGroup.Description = null;
            if (string.IsNullOrWhiteSpace(productGroup.Features))
                productGroup.Features = null;
            if (string.IsNullOrWhiteSpace(productGroup.SubTitle))
                productGroup.SubTitle = null;
            productGroup.LastUpdatedOn = DateTime.Now;
            productGroup.LastUpdatedBy = _userContext.loggedInUser.Id;
            Update(productGroup);
            await SaveChangesAsync();
        }

        public async Task<bool> IsProductGroupExistByName(string name)
        {
            return await AnyAsync(x => !x.IsDeleted && x.Name.ToLower().Equals(name.ToLower()));
        }

        public async Task<bool> IsProductGroupIdExistAsync(int id)
        {
            return await AnyAsync(x => x.Id.Equals(id) && !x.IsDeleted);
        }

        private async Task IsProductGroupDataValidAsync(ProductGroup productGroup)
        {
            if(string.IsNullOrWhiteSpace(productGroup.Name))
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product Group", "Name"));
            if (await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(productGroup.Id) && x.Name.ToLower().Equals(productGroup.Name.ToLower())))
                throw new ApiException(System.Net.HttpStatusCode.Conflict, string.Format(Constants.AleadyExistExceptionMessage, "Product Group", "Name", productGroup.Name));
            if (productGroup.CategoryId <= 0)
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Product Group", "Category Id"));
        }
    }
}
