using Core;
using Core.Authentication;
using Data.Models;
using Dto;
using Dto.Common;
using Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public async Task<PaginatedList<ReviewDto>> GetReviewesAsync(ReviewFilterDto reviewFilterDto)
    {
        // create paginated Address List
        var paginatedReviewList = new PaginatedList<ReviewDto>();

        //create Predicates
        var reviewFilterPredicate = PradicateBuilder.True<Review>();
        var productFilterPredicate = PradicateBuilder.True<Product>();

        //Apply Review is Deleted filter
        reviewFilterPredicate = reviewFilterPredicate.And(x => !x.IsDeleted);

        //Apply Product is Deleted filter
        productFilterPredicate = productFilterPredicate.And(x => !x.IsDeleted);

        //Get Review filters
        reviewFilterPredicate = ApplyReviewFilters(reviewFilterPredicate, reviewFilterDto);

        //Get Product filters
        productFilterPredicate = ApplyProductFilters(productFilterPredicate, reviewFilterDto);

        //Apply filters
        var reviewQueyable = _reviewRepo.GetQueyable().Where(reviewFilterPredicate);
        var productQueyable = _productRepo.GetQueyable().Where(productFilterPredicate);

        //join
        IQueryable<ReviewDto> reviewQuery = reviewQueyable
            .Join(
                productQueyable,
                review => review.ProductId,
                product => product.Id,
                (review, product) => new ReviewDto()
                {
                    Id = review.Id,
                    ProductID = review.ProductId.Value,
                    ProductName = product.Name,
                    Rating = review.Rating.Value,
                    Review = review.Review1,
                    AddedBy = review.AddedBy.Value
                }
            );

        //ApplyGenericFilter
        reviewQuery = ApplyGenericFilters(reviewQuery, reviewFilterDto);

        //OrderBy
        reviewQuery = ApplyOrderByFilter(reviewQuery, reviewFilterDto);

        //FatchTotalCount
        paginatedReviewList.Count = await reviewQuery.CountAsync();

        //Pagination
        reviewQuery = ApplyPaginationFilter(reviewQuery, reviewFilterDto);

        //FatchItems
        paginatedReviewList.Items = await reviewQuery.ToListAsync();

        return paginatedReviewList;
    }

    private IQueryable<ReviewDto> ApplyGenericFilters(IQueryable<ReviewDto> reviewQuery, ReviewFilterDto reviewFilterDto)
    {
        //Generic filters
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<ReviewDto>();
            var filterText = reviewFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.ProductID.ToString(), $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.ProductName, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Review, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Rating.ToString(), $"%{filterText}%"));

            //Apply generic filters
            return reviewQuery.Where(genericFilterPredicate);
        }

        return reviewQuery;
    }

    public async Task<ReviewDto> GetReviewByIDAsync(int id)
    {
        var reviewQueyable = _reviewRepo.GetQueyable().Where(x => x.Id == id && !x.IsDeleted);
        var productQueyable = _productRepo.GetQueyable().Where(x => !x.IsDeleted);

        var reviewQuery = await reviewQueyable
            .Join(
                productQueyable,
                review => review.ProductId,
                product => product.Id,
                (review, product) => new ReviewDto()
                {
                    Id = review.Id,
                    ProductID = review.ProductId.Value,
                    ProductName = product.Name,
                    Rating = review.Rating.Value,
                    Review = review.Review1,
                    AddedBy = review.AddedBy.Value

                }
            ).FirstOrDefaultAsync();

        if (reviewQuery == null)
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Review", "Id", id));

        return reviewQuery;
    }

    public async Task<PaginatedList<ReviewDto>> GetReviewByProductIdAsync(int productId, ReviewFilterDto reviewFilterDto)
    {
        if (!await _productRepo.IsProductIdExistAsync(productId))
            throw new ApiException(HttpStatusCode.BadRequest, string.Format(Constants.NotExistExceptionMessage, "Review ", "Product Id", productId));

        // create paginated Address List
        var paginatedReviewList = new PaginatedList<ReviewDto>();

        //create Predicates
        var reviewFilterPredicate = PradicateBuilder.True<Review>();
        var productFilterPredicate = PradicateBuilder.True<Product>();

        //Apply Review Product id filter
        reviewFilterPredicate = reviewFilterPredicate.And(x => !x.IsDeleted);
        reviewFilterPredicate = reviewFilterPredicate.And(x => x.ProductId.Equals(productId));
        productFilterPredicate = productFilterPredicate.And(x => !x.IsDeleted);

        //Get Review filters
        reviewFilterPredicate = ApplyReviewFilters(reviewFilterPredicate, reviewFilterDto);

        //Get Product filters
        productFilterPredicate = ApplyProductFilters(productFilterPredicate, reviewFilterDto);

        //Apply filters
        var reviewQueyable = _reviewRepo.GetQueyable().Where(reviewFilterPredicate);
        var productQueyable = _productRepo.GetQueyable().Where(productFilterPredicate);

        //join
        IQueryable<ReviewDto> reviewQuery = reviewQueyable
            .Join(
                productQueyable,
                review => review.ProductId,
                product => product.Id,
                (review, product) => new ReviewDto()
                {
                    Id = review.Id,
                    ProductID = review.ProductId.Value,
                    ProductName = product.Name,
                    Rating = review.Rating.Value,
                    Review = review.Review1,
                    AddedBy = review.AddedBy.Value
                }
            );

        //ApplyGenericFilter
        reviewQuery = ApplyGenericFilters(reviewQuery, reviewFilterDto);

        //OrderBy
        reviewQuery = ApplyOrderByFilter(reviewQuery, reviewFilterDto);

        //Get a Review List
        List<ReviewDto> reviewList = await reviewQuery.ToListAsync();

        // Find a Review using UserID
        var index = reviewList.FindIndex(x => x.AddedBy.Equals(_userContext.loggedInUser.Id));

        // Swap A Review To First
        if (index > -1)
        {
            var temp = reviewList[0];
            reviewList[0] = reviewList[index];
            reviewList[index] = temp;
        }

        //FatchTotalCount
        paginatedReviewList.Count = reviewList.Count();

        //Pagination
        reviewQuery = ApplyPaginationFilter(reviewQuery, reviewFilterDto);

        //FatchItems
        paginatedReviewList.Items = reviewList.ToList();

        return paginatedReviewList;

        //var reviewQueyable = _reviewRepo.GetQueyable();
        //var productQueyable = _productRepo.GetQueyable();

        //IQueryable<ReviewDto> reviewQuery = reviewQueyable
        //    .Join(
        //        productQueyable,
        //        review => review.ProductId,
        //        product => product.Id,
        //        (review, product) => new
        //        {
        //            review.Id,
        //            review.ProductId,
        //            review.Rating,
        //            review.Review1,
        //            review.IsDeleted,
        //            productDeleted = product.IsDeleted,
        //            ProductName = product.Name,
        //            review.AddedBy,
        //        }
        //    )
        //    .Where(x => x.ProductId == productId && !x.IsDeleted && !x.productDeleted)
        //    .Select(x => new ReviewDto()
        //    {
        //        Id = x.Id,
        //        ProductID = x.ProductId.Value,
        //        ProductName = x.ProductName,
        //        Rating = x.Rating.Value,
        //        Review = x.Review1,
        //        AddedBy = x.AddedBy.Value,
        //    });

        ////GenericTextFilterQuery
        //if (!string.IsNullOrWhiteSpace(reviewFilterDto.GenericTextFilter))
        //    reviewQuery = reviewQuery.Where(x =>
        //                x.ProductName.ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()) ||
        //                x.Rating.ToString().ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()) ||
        //                (!string.IsNullOrWhiteSpace(x.Review) && x.Review.ToLower().Contains(reviewFilterDto.GenericTextFilter.ToLower()))
        //            );

        //// FieldTextFilterQuery
        //if (!string.IsNullOrWhiteSpace(reviewFilterDto.ProductNameFilterText))
        //    reviewQuery = reviewQuery.Where(x => x.ProductName.ToLower().Contains(reviewFilterDto.ProductNameFilterText.ToLower()));
        //if (!string.IsNullOrWhiteSpace(reviewFilterDto.RatingFilterText))
        //    reviewQuery = reviewQuery.Where(x => x.Rating.ToString().Contains(reviewFilterDto.RatingFilterText.ToString()));
        //if (!string.IsNullOrWhiteSpace(reviewFilterDto.ReviewFilterText))
        //    reviewQuery = reviewQuery.Where(x => !string.IsNullOrWhiteSpace(x.Review) && x.Review.ToLower().Contains(reviewFilterDto.ReviewFilterText.ToLower()));

        ////OrderByQuery
        //if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByProductIdValue, StringComparison.OrdinalIgnoreCase))
        //    reviewQuery = reviewQuery.OrderBy(x => x.ProductID);
        //else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByProductNameValue, StringComparison.OrdinalIgnoreCase))
        //    reviewQuery = reviewQuery.OrderBy(x => x.ProductName);
        //else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByRatingValue, StringComparison.OrdinalIgnoreCase))
        //    reviewQuery = reviewQuery.OrderBy(x => x.Rating);
        //else if (!string.IsNullOrWhiteSpace(reviewFilterDto.OrderByField) && reviewFilterDto.OrderByField.ToLower().Equals(Constants.OrderByReviewValue, StringComparison.OrdinalIgnoreCase))
        //    reviewQuery = reviewQuery.OrderBy(x => x.Review);
        //else
        //    reviewQuery = reviewQuery.OrderBy(x => x.Id);

        ////Pagination
        //if (reviewFilterDto.IsPagination)
        //    reviewQuery = reviewQuery.Skip((reviewFilterDto.PageNo - 1) * reviewFilterDto.PageSize).Take(reviewFilterDto.PageSize);

        //List<ReviewDto> reviewList = await reviewQuery.ToListAsync();

        //var index = reviewList.FindIndex(x => x.AddedBy.Equals(_userContext.loggedInUser.Id));
        //if (index > -1)
        //{
        //    var temp = reviewList[0];
        //    reviewList[0] = reviewList[index];
        //    reviewList[index] = temp;
        //}
        //return reviewList;
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

    private Expression<Func<Product, bool>> ApplyProductFilters(Expression<Func<Product, bool>> productFilterPredicate, ReviewFilterDto reviewFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.ProductNameFilterText))
            productFilterPredicate = productFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{reviewFilterDto.ProductNameFilterText.Trim()}%"));

            return productFilterPredicate;
    }

    private Expression<Func<Review, bool>> ApplyReviewFilters(Expression<Func<Review, bool>> reviewFilterPredicate, ReviewFilterDto reviewFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.RatingFilterText))
            reviewFilterPredicate = reviewFilterPredicate.And(x => EF.Functions.ILike(x.Rating.Value.ToString(), $"%{reviewFilterDto.RatingFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(reviewFilterDto.ReviewFilterText))
            reviewFilterPredicate = reviewFilterPredicate.And(x => EF.Functions.ILike(x.Review1, $"%{reviewFilterDto.ReviewFilterText.Trim()}%"));

        return reviewFilterPredicate;
    }

    private IQueryable<ReviewDto> ApplyOrderByFilter(IQueryable<ReviewDto> reviewQuery, ReviewFilterDto reviewFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<ReviewDto, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByUserNameValue, x => x.ProductID},
            { Constants.OrderByNameValue, x => x.ProductName ?? "" },
            { Constants.OrderByLine1Value, x => x.Rating},
            { Constants.OrderByLine2Value, x => x.Review ?? "" }
        };

        if (!orderByMappings.TryGetValue(reviewFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        reviewQuery = reviewFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? reviewQuery.OrderByDescending(orderByExpression)
            : reviewQuery.OrderBy(orderByExpression);

        return reviewQuery;
    }

    private IQueryable<ReviewDto> ApplyPaginationFilter(IQueryable<ReviewDto> reviewQuery, ReviewFilterDto reviewFilterDto)
    {
        if (reviewFilterDto.IsPagination)
            reviewQuery = reviewQuery.Skip((reviewFilterDto.PageNo - 1) * reviewFilterDto.PageSize).Take(reviewFilterDto.PageSize);

        return reviewQuery;
    }
}
