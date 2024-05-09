using EduPortal.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface ISingletonRepository<TEntity, TKey> where TEntity : class, IEntity
    {
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        void Remove(TEntity entity);


    }
}
