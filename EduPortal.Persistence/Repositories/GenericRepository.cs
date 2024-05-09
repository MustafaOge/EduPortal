using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Abstractions;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();

        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await _dbSet.AddRangeAsync(items);
        }



        public async Task AddAsync(TEntity entity)
        {
            var state = _context.Entry(entity).State;

            _dbSet.AddAsync(entity);

            _context.Entry(entity).State = EntityState.Added;


            var state2 = _context.Entry(entity).State;


            _context.SaveChanges();

        }


        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);

        }
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> items)
        {
            _dbSet.RemoveRange(items);
        }

        public EntityEntry<TEntity> GetEntry(TEntity entity)
        {
            return _dbSet.Entry(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void Detach(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}





//using EduPortal.Application.Interfaces.Repositories;
//using EduPortal.Domain.Abstractions;
//using EduPortal.Persistence.context;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace EduPortal.Persistence.Repositories
//{
//    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity
//    {
//        protected readonly AppDbContext _context;
//        protected readonly DbSet<TEntity> _dbSet;

//        public GenericRepository(AppDbContext context)
//        {
//            _context = context;
//            _dbSet = _context.Set<TEntity>();
//        }

//        public async Task<TEntity> GetByIdAsync(TKey id)
//        {
//            return await _dbSet.FindAsync(id);
//        }

//        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
//        {
//            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();

//        }
//        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
//        {
//            return _dbSet.Where(expression);
//        }

//        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
//        {
//            return await _dbSet.AnyAsync(expression);
//        }

//        public async Task AddRangeAsync(IEnumerable<TEntity> items)
//        {
//            await _dbSet.AddRangeAsync(items);
//        }



//        //public async Task AddAsync(TEntity entity)
//        //{
//        //    await _dbSet.AddAsync(entity);
//        //}

//        public async Task AddAsync(TEntity entity)
//        {

//            await _dbSet.AddAsync(entity);
//            _context.Entry(entity).State = EntityState.Added;

//        }

//        //public async Task AddAsync(TEntity entity)
//        //{
//        //    var state = _context.Entry(entity).State;

//        //    _context.Entry(entity).State = EntityState.Added;

//        //    var state2 = _context.Entry(entity).State;

//        //     _context.SaveChangesAsync();
//        //}




//        public void Update(TEntity entity)
//        {
//            _dbSet.Update(entity);

//        }
//        public void Remove(TEntity entity)
//        {
//            _dbSet.Remove(entity);
//        }

//        public void RemoveRange(IEnumerable<TEntity> items)
//        {
//            _dbSet.RemoveRange(items);
//        }
//    }
//}
