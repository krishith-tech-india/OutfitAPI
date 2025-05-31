using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IReviewService
{
    Task<List<ReviewDto>> GetReviewesAsync(ReviewFilterDto reviewFilterDto);
    Task<ReviewDto> GetReviewByIDAsync(int id);
    Task<List<ReviewDto>> GetReviewByProductIdAsync(int ProductId, ReviewFilterDto reviewFilterDto);
    Task InsertReviewAsync(ReviewDto reviewDto);
    Task UpdateReviewAsync(int id, ReviewDto reviewDto);
    Task DeleteReviewAsync(int id);
}
