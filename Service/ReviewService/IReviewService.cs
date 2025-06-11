using Dto;
using Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IReviewService
{
    Task<PaginatedList<ReviewDto>> GetReviewesAsync(ReviewFilterDto reviewFilterDto);
    Task<ReviewDto> GetReviewByIDAsync(int id);
    Task<PaginatedList<ReviewDto>> GetReviewByProductIdAsync(int ProductId, ReviewFilterDto reviewFilterDto);
    Task InsertReviewAsync(ReviewDto reviewDto);
    Task UpdateReviewAsync(int id, ReviewDto reviewDto);
    Task DeleteReviewAsync(int id);
}
