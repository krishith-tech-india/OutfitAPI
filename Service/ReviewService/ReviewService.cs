using Core;
using Core.Authentication;
using Dto;
using Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service;

public class ReviewService : IReviewService
{
    private readonly IReviewRepo _reviewRepo;
    private readonly IReviewMapper _reviewMapper;
    private readonly IProductRepo _productRepo;
    private readonly IUserContext _userContext;

    public ReviewService(
        IReviewRepo reviewRepo,
        IReviewMapper reviewMapper,
        IProductRepo productRepo,
        IUserContext userContext
    )
    {
        _reviewRepo = reviewRepo;
        _reviewMapper = reviewMapper;
        _productRepo = productRepo;
        _userContext = userContext;
    }

    public async Task<List<ReviewDto>> GetReviewesAsync(ReviewFilterDto reviewFilterDto)
    {
        var reviewQueyable = _reviewRepo.GetQueyable();
        var productQueyable = _productRepo.GetQueyable();

        IQueryable<ReviewDto> reviewQuery = reviewQueyable
            .Join(
                productQueyable,
                review => review.ProductId,
                product => product.Id,
                (review, product) => new
                {
                    review.Id,
                    review.ProductId,
                    review.Rating,
                    review.Review1,
                    review.IsDeleted,
                    productDeleted = product.IsDeleted,
                    ProductName = product.Name,
                }
            )
            .Where(x => !x.IsDeleted && !x.productDeleted)
            .Select(x => new ReviewDto()
            {
                Id = x.Id,
                ProductID = x.ProductId.Value,
                ProductName = x.ProductName,
                Rating = x.Rating.Value,
                Review = x.Review1
            });

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.GenericTextFilter))
            reviewQuery = reviewQuery.Where(x =>
                        x.ProductName.ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()) ||
                        x.Rating.ToString().ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Review) && x.Review.ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()))
                    );

        // FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.ProductNameFilterText))
            reviewQuery = reviewQuery.Where(x => x.ProductName.ToLower().Contains(reviewFilterDto.ProductNameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.RatingFilterText))
            reviewQuery = reviewQuery.Where(x => x.Rating.ToString().Contains(reviewFilterDto.RatingFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.ReviewFilterText))
            reviewQuery = reviewQuery.Where(x => !string.IsNullOrWhiteSpace(x.Review) && x.Review.ToLower().Contains(reviewFilterDto.ReviewFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByProductIdValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.ProductID);
        else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByProductNameValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.ProductName);
        else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByRatingValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.Rating);
        else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByReviewValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.Review);
        else
            reviewQuery = reviewQuery.OrderBy(x => x.Id);

        //Pagination
        if (reviewFilterDto.IsPagination)
            reviewQuery = reviewQuery.Skip((reviewFilterDto.PageNo - 1) * reviewFilterDto.PageSize).Take(reviewFilterDto.PageSize);

        return await reviewQuery.ToListAsync();
    }

    public async Task<ReviewDto> GetReviewByIDAsync(int id)
    {
        var reviewQueyable = _reviewRepo.GetQueyable();
        var productQueyable = _productRepo.GetQueyable();

        var reviewQuery = await reviewQueyable
            .Join(
                productQueyable,
                review => review.ProductId,
                product => product.Id,
                (review, product) => new
                {
                    review.Id,
                    review.ProductId,
                    review.Rating,
                    review.Review1,
                    review.IsDeleted,
                    productDeleted = product.IsDeleted,
                    ProductName = product.Name,
                }
            )
            .Where(x => x.Id == id && !x.IsDeleted && !x.productDeleted)
            .Select(x => new ReviewDto()
            {
                Id = x.Id,
                ProductID = x.ProductId.Value,
                ProductName = x.ProductName,
                Rating = x.Rating.Value,
                Review = x.Review1
            }).FirstOrDefaultAsync();

        if (reviewQuery == null)
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Review", "Id", id));

        return reviewQuery;
    }

    public async Task<List<ReviewDto>> GetReviewByProductIdAsync(int ProductId, ReviewFilterDto reviewFilterDto)
    {
        if (!await _productRepo.IsProductIdExistAsync(ProductId))
            throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Review ", "Product Id", ProductId));

        var reviewQueyable = _reviewRepo.GetQueyable();
        var productQueyable = _productRepo.GetQueyable();

        IQueryable<ReviewDto> reviewQuery = reviewQueyable
            .Join(
                productQueyable,
                review => review.ProductId,
                product => product.Id,
                (review, product) => new
                {
                    review.Id,
                    review.ProductId,
                    review.Rating,
                    review.Review1,
                    review.IsDeleted,
                    productDeleted = product.IsDeleted,
                    ProductName = product.Name,
                    review.AddedBy,
                }
            )
            .Where(x => x.ProductId == ProductId && !x.IsDeleted && !x.productDeleted)
            .Select(x => new ReviewDto()
            {
                Id = x.Id,
                ProductID = x.ProductId.Value,
                ProductName = x.ProductName,
                Rating = x.Rating.Value,
                Review = x.Review1,
                AddedBy = x.AddedBy.Value,
            });

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.GenericTextFilter))
            reviewQuery = reviewQuery.Where(x =>
                        x.ProductName.ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()) ||
                        x.Rating.ToString().ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Review) && x.Review.ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()))
                    );

        // FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.ProductNameFilterText))
            reviewQuery = reviewQuery.Where(x => x.ProductName.ToLower().Contains(reviewFilterDto.ProductNameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.RatingFilterText))
            reviewQuery = reviewQuery.Where(x => x.Rating.ToString().Contains(reviewFilterDto.RatingFilterText.ToString()));
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.ReviewFilterText))
            reviewQuery = reviewQuery.Where(x => !string.IsNullOrWhiteSpace(x.Review) && x.Review.ToLower().Contains(reviewFilterDto.ReviewFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByProductIdValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.ProductID);
        else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByProductNameValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.ProductName);
        else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByRatingValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.Rating);
        else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByReviewValue, StringComparison.OrdinalIgnoreCase))
            reviewQuery = reviewQuery.OrderBy(x => x.Review);
        else
            reviewQuery = reviewQuery.OrderBy(x => x.Id);

        //Pagination
        if (reviewFilterDto.IsPagination)
            reviewQuery = reviewQuery.Skip((reviewFilterDto.PageNo - 1) * reviewFilterDto.PageSize).Take(reviewFilterDto.PageSize);

        List<ReviewDto> reviewList = await reviewQuery.ToListAsync();

        var index = reviewList.FindIndex(x => x.AddedBy.Equals(_userContext.loggedInUser.Id));
        if (index > -1)
        {
            var temp = reviewList[0];
            reviewList[0] = reviewList[index];
            reviewList[index] = temp;
        }
        return reviewList;
    }

    public async Task InsertReviewAsync(ReviewDto reviewDto)
    {
        if (!await _productRepo.IsProductIdExistAsync(reviewDto.ProductID))
            throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Review ", "Product Id", reviewDto.ProductID));
        var review = _reviewMapper.GetEntity(reviewDto);
        await _reviewRepo.InsertReviewAsync(review);
    }

    public async Task UpdateReviewAsync(int id,ReviewDto reviewDto)
    {
        if (!await _productRepo.IsProductIdExistAsync(reviewDto.ProductID))
            throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Review ", "Product Id", reviewDto.ProductID));
        var review = await _reviewRepo.GetReviewByIDAsync(id);
        review.ProductId = reviewDto.ProductID;
        review.Rating = reviewDto.Rating;
        review.Review1 = reviewDto.Review;
        await _reviewRepo.UpdateReviewAsync(review);
    }

    public async Task DeleteReviewAsync(int id)
    {
        var review = await _reviewRepo.GetReviewByIDAsync(id);
        review.IsDeleted = true;
        await _reviewRepo.UpdateReviewAsync(review);
    }
}
