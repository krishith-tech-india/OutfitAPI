using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repo
{
    internal class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        //private readonly DBContext _context;
        private DbSet<T> _db;
        //public Repository(
        //        DBContext context
        //    )
        //{
        //    _context = context;
        //    _db = _context.Set<T>();
        //}

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await _db.AnyAsync(expression);
        }

        //public IDbContextTransaction BeginTransaction()
        //{
        //    return _context.Database.BeginTransaction();
        //}

        public async Task CompleTransaction(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.CommitAsync();
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _db.FindAsync(id);
        }

        public IQueryable<T> GetQueyable()
        {
            return _db.AsQueryable();
        }

        public async Task InsertAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        //public async Task SaveChangesAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}

        public IQueryable<T> Select(Expression<Func<T, bool>> expression)
        {
            return _db.Where(expression);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }
    }
}
