using EduPortal.Domain.Abstractions;
using EduPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    /// <summary>
    /// Generic repository interface used for general database operations.
    /// </summary>
    /// <typeparam name="T">Type of the database entity.</typeparam>
    public interface IGenericRepository<TEntity, TKey> where TEntity : class, IEntity
    {
        /// <summary>
        /// Asynchronously retrieves an entity with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        Task<TEntity> GetByIdAsync(TKey id);

        /// <summary>
        /// Retrieves all entities without any conditions.
        /// </summary>
        /// <returns>List for all entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Queries entities that meet the specified conditions.
        /// </summary>
        /// <param name="expression">Filtering conditions.</param>
        /// <returns>Query for entities that meet the specified conditions.</returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Checks if there is any entity that meets the specified conditions.
        /// </summary>
        /// <param name="expression">Filtering conditions.</param>
        /// <returns>"True" if there is any entity that meets the specified conditions, otherwise "false".</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Asynchronously adds the specified entities.
        /// </summary>
        /// <param name="items">Collection of entities to be added.</param>
        Task AddRangeAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// Asynchronously adds the specified entity.
        /// </summary>
        /// <param name="entity">Entity to be added.</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">Entity to be removed.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes the specified entities.
        /// </summary>
        /// <param name="items">Collection of entities to be removed.</param>
        void RemoveRange(IEnumerable<TEntity> items);
        /// <summary>
        /// Gets the entity entry for the specified entity.
        /// </summary>
        /// <param name="entity">The entity for which to get the entry.</param>
        /// <returns>The entity entry for the specified entity.</returns>
        EntityEntry<TEntity> GetEntry(TEntity entity);

        /// <summary>
        /// Updates a range of entities.
        /// </summary>
        /// <param name="entities">The collection of entities to be updated.</param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates async the specified entity.
        /// </summary>
        /// <param name="entity">Entity to be updated async.</param>
        Task UpdateAsync(TEntity entity);

        void Detach(TEntity entity);
    }
}
