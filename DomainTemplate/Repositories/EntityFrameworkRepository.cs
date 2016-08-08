using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DomainTemplate.Helpers;
using DomainTemplate.Interfaces;

namespace DomainTemplate.Repositories
{
    public abstract class EntityFrameworkRepository<TEntity, TContext>
        : IRepository<TEntity>, IDisposable
        where TEntity : class, IEntity
        where TContext : class, IContext
    {
        protected bool _disposed;
        protected DbContext _context;

        protected EntityFrameworkRepository(IUnitOfWork uow)
        {
            _context = uow.AssignContext<TContext>() as DbContext;

            if (_context == null)
                throw new InvalidOperationException($"{typeof(DbContext).FullName} context not found");
        }

        public virtual IEnumerable<TEntity> GetAll() => _context.Set<TEntity>();

        public virtual TEntity GetById(int id) => _context.Set<TEntity>().Find(id);

        public virtual void Insert(TEntity entity)
        {
            UpdateOrInsertEntity(entity);
        }

        public virtual void Update(TEntity entity)
        {
            UpdateOrInsertEntity(entity);
        }

        public virtual void UpdateOrInsertEntity(TEntity entity)
        {
            entity.State = entity.Id == default(int) ? State.Added : State.Modified;

            if (!CheckIfAttached(entity))
                _context.Set<TEntity>().Attach(entity);

            _context.ApplyStateChanges();
        }

        public virtual void UpdateOrInsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                UpdateOrInsertEntity(entity);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void DeleteById(int id)
        {
            var entity = GetById(id);
            Delete(entity);
        }

        private bool CheckIfAttached(TEntity entity)
        {
            return _context.Set<TEntity>().Local.Any(x => x == entity);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (!disposing) return;
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            _disposed = true;
        }
    }
}
