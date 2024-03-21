using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EduPortal.Domain.Entities;

namespace EduPortal.Persistence.Interceptors
{
    internal class SaveChangesInterceptor(IHttpContextAccessor contextAccessor)
        : Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor
    {
        readonly Dictionary<EntityState, Action<DbContext,
            //BaseEntity, 
            string>> _actions = new()
        {
            //{ EntityState.Added, AddEntity },
            //{ EntityState.Modified, ModifiedEntity }
        };


        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var dbContext = eventData.Context;

            if (dbContext is null) return base.SavingChanges(eventData, result);


            var context = contextAccessor.HttpContext;
            var userId = context?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            userId ??= "10";


            //var entities = dbContext.ChangeTracker.Entries<BaseEntity>().ToList();
            //foreach (var entry in entities)
            //{
            //    var baseEntity = entry.Entity;

            //    _actions[entry.State](dbContext, baseEntity, userId);

            //    //switch (entry.State)
            //    //{
            //    //    case EntityState.Added:

            //    //        AddEntity(dbContext, baseEntity, userId);

            //    //        break;
            //    //    case EntityState.Modified:
            //    //        ModifiedEntity(dbContext, baseEntity, userId);

            //    //        break;
            //    //}
            //}

            return base.SavingChanges(eventData, result);
        }

        //private static void ModifiedEntity(DbContext dbContext, BaseEntity baseEntity, string userId)
        //{
        //    dbContext.Entry(baseEntity).Property(x => x.Created).IsModified = false;
        //    dbContext.Entry(baseEntity).Property(x => x.CreatedByUser).IsModified = false;
        //    baseEntity.UpdatedByUser = Convert.ToInt32(userId);
        //    baseEntity.Updated = DateTime.Now;
        //}

        //private static void AddEntity(DbContext dbContext, BaseEntity baseEntity, string userId)
        //{
        //    dbContext.Entry(baseEntity).Property(x => x.Updated).IsModified = false;
        //    dbContext.Entry(baseEntity).Property(x => x.UpdatedByUser).IsModified = false;
        //    baseEntity.Created = DateTime.Now;
        //    baseEntity.CreatedByUser = Convert.ToInt32(userId);
        //}
    }
}