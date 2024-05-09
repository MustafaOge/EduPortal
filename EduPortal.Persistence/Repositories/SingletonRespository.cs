using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Abstractions;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{
    public class SingletonRepository<TEntity, TKey> : ISingletonRepository<TEntity, TKey> where TEntity : class, IEntity
    {

        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public SingletonRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}