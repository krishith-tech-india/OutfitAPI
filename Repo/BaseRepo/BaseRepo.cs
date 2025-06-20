﻿using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Data.Contexts;

namespace Repo
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly OutfitDBContext _context;
        private DbSet<T> _db;

        public BaseRepo(
               OutfitDBContext context
           )
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.AnyAsync(expression);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task CommitTransaction(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }
        }

        //public void Delete(T entity)
        //{
        //    _db.Remove(entity);
        //}

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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        //public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        //{
        //    return _db.Where(expression);
        //}

        public void Update(T entity)
        {
            _db.Update(entity);
        }
    }
}
