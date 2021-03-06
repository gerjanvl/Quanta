﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quanta.Domain;

namespace Quanta.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _entitySet;
        private readonly QuantaContext _context;

        public Repository(QuantaContext context)
        {
            _context = context;
            _entitySet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> All()
        {
            return _entitySet.AsQueryable();
        }

        public virtual void Add(TEntity entity)
        {
            _entitySet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            _entitySet.Remove(entity);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}