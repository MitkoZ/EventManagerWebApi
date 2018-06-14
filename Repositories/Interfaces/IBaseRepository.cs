using DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : IEntity
    {
        List<TEntity> GetAll(Func<TEntity, bool> filter = null);

        int Save(TEntity item);

        int Create(TEntity item);

        int Update(TEntity item, Func<TEntity, bool> filter);

        int Delete(int id);
    }
}
