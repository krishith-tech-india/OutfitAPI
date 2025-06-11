using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class ReviewRepo : BaseRepo<Review>, IReviewRepo
{
    private readonly IUserContext _userContext;
    public ReviewRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task<Review> GetReviewByIDAsync(int id)
    {
        var ProductReview = await GetByIdAsync(id);
        if (ProductReview == null || ProductReview.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Review ", "Id", id));
        return ProductReview;
    }

    public async Task InsertReviewAsync(Review review)
    {
        await IsReviewDataValidAsync(review);
        if (string.IsNullOrWhiteSpace(review.Review1))
            review.Review1 = null;
        review.AddedOn = DateTime.Now;
        review.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(review);
        await SaveChangesAsync();
    }

    public async Task UpdateReviewAsync(Review review)
    {
        await IsReviewDataValidAsync(review);
        if (string.IsNullOrWhiteSpace(review.Review1))
            review.Review1 = null;
        review.LastUpdatedOn = DateTime.Now;
        review.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(review);
        await SaveChangesAsync();
    }

    private async Task IsReviewDataValidAsync(Review review)
    {
        if(await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(review.Id) && (x.ProductId.Equals(review.ProductId) && x.AddedBy.Equals(_userContext.loggedInUser.Id))))
            throw new ApiException(System.Net.HttpStatusCode.Conflict, string.Format(Constants.AleadyExistExceptionMessage, "Review", "Product Id With", "User ID"));
        if (review.Rating <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Review ", "Rating"));
        if (review.Rating >= 5)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.ValidFieldExceptionMessage, "Review"));
    }
}
