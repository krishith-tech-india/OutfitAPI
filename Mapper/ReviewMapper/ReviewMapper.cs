using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class ReviewMapper : IReviewMapper
{
    public Review GetEntity(ReviewDto reviewDto)
    {
        return new Review
        {
            ProductId = reviewDto.ProductID,
            Rating = reviewDto.Rating,
            Review1 = reviewDto.Review,
        };
    }

    public ReviewDto GetReviewDto(Review review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            ProductID = review.ProductId.Value,
            Rating = review.Rating.Value,
            Review = review.Review1
        };
    }
}
