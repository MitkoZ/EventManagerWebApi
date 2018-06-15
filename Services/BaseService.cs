using DataAccess.Interfaces;
using Repositories;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services
{
    public abstract class BaseService<TEntity, Repository>
        where TEntity : class, IEntity
        where Repository : BaseRepository<TEntity>
    {
        private readonly IValidationDictionary validationDictionary;
        protected Repository repository;
        public BaseService(IValidationDictionary validationDictionary, Repository repository)
        {
            this.validationDictionary = validationDictionary;
            this.repository = repository;
        }

        public void AddValidationError(string key, string errorMessage)
        {
            this.validationDictionary.AddError(key, errorMessage);
        }

        public bool PreValidate()
        {
            if (!this.validationDictionary.IsValid)
            {
                return false;
            }
            return true;
        }

        public List<TEntity> GetAll(Func<TEntity, bool> filter = null)
        {
            return this.repository.GetAll(filter);
        }

        public bool Save(TEntity entity)
        {
            return this.repository.Save(entity) > 0;
        }

        public bool Delete(int id)
        {
            return this.repository.Delete(id) > 0;
        }
    }
}
