using DataAccess;
using DataAccess.Interfaces;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class, IEntity
    {
        private EventManagerDbContext context;

        protected DbSet<TEntity> DBSet
        {
            get
            {
                return context.Set<TEntity>();
            }
        }

        public BaseRepository()
        {
            this.context = new EventManagerDbContext();
        }

        public List<TEntity> GetAll(Func<TEntity, bool> filter = null)
        {
            if (filter != null)
            {
                return context.Set<TEntity>().Where(filter).ToList();
            }

            return context.Set<TEntity>().ToList();
        }

        public int Create(TEntity item)
        {
            context.Set<TEntity>().Add(item);
            return context.SaveChanges();
        }

        public int Update(TEntity item, Func<TEntity, bool> filter)
        {
            var local = context.Set<TEntity>()
                           .Local
                           .FirstOrDefault(filter);
            context.Entry(item).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public int Save(TEntity item)
        {
            if (item.Id == 0)
            {
                return Create(item);
            }
            else
            {
                return Update(item, x => x.Id == item.Id);
            }
        }

        public int Delete(int id)
        {
            TEntity dbItem = context.Set<TEntity>().Find(id);
            if (dbItem != null)
            {
                context.Set<TEntity>().Remove(dbItem);
            }
            return context.SaveChanges();
        }
    }
}
