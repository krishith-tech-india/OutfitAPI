using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IReviewRepo : IBaseRepo<Review>
{
    Task<Review> GetReviewByIDAsync(int id);
    Task InsertReviewAsync(Review review);
    Task UpdateReviewAsync(Review review);
}
