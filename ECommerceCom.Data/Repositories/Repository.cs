﻿using ECommerceCom.Data.Context;
using ECommerceCom.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ECommerceComDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ECommerceComDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate);
        }
        public TEntity GeyByID(int id)
        {
            return _dbSet.Find(id);
        }
        public void Update(TEntity entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _dbSet.Update(entity);
        }
    }
}
