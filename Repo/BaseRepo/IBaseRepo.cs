using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Repo
{
    public interface IBaseRepo<T>
    {
        IDbContextTransaction BeginTransaction();
        Task CommitTransaction(IDbContextTransaction transaction);
        Task<T?> GetByIdAsync(object id);
        IQueryable<T> GetQueyable();
        Task InsertAsync(T entity);
        void Update(T entity);
        //void Delete(T entity);
        //IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task SaveChangesAsync();
    }
}
