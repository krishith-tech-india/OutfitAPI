using Core;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize]
        [HttpPost("GetReviewes/")]
        public async Task<ApiResponse> GetReviewes([FromBody] ReviewFilterDto reviewFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _reviewService.GetReviewesAsync(reviewFilterDto));
        }

        [Authorize]
        [HttpGet("GetReviewByID/{Id}")]
        public async Task<ApiResponse> GetReviewByID(int Id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _reviewService.GetReviewByIDAsync(Id));
        }

        [Authorize]
        [HttpPost("GetReviewByProductId/{ProductId}")]
        public async Task<ApiResponse> GetReviewByProductId(int ProductId, [FromBody] ReviewFilterDto reviewFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _reviewService.GetReviewByProductIdAsync(ProductId, reviewFilterDto));
        }

        [Authorize]
        [HttpPost("InsertReview/")]
        public async Task<ApiResponse> InsertReview([FromBody] ReviewDto reviewDto)
        {
            await _reviewService.InsertReviewAsync(reviewDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("UpdateReview/{id}")]
        public async Task<ApiResponse> UpdateReview(int id,[FromBody] ReviewDto reviewDto)
        {
            await _reviewService.UpdateReviewAsync(id,reviewDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteReview/{id}")]
        public async Task<ApiResponse> DeleteReview(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
