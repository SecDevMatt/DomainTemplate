using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using DomainTemplate.Interfaces;

namespace DomainTemplate.Helpers
{
    public class ModelManager : IUnitOfWork
    {
        private bool _disposed;
        public ICollection<IContext> Contexts { get; private set; } = new List<IContext>();

        public void AddContext(IContext context)
        {
            if (Contexts.Any(x => x.GetType() == context.GetType()))
                throw new ArgumentException($"{nameof(Contexts)} already contains a context of type: {context.GetType()}");

            Contexts.Add(context);
        }

        public IContext AssignContext<TContext>() where TContext : class, IContext
        {
            try
            {
                return Contexts.Single(x => x.GetType() == typeof(TContext));
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"No context of type: {typeof(TContext)}");

            }
        }

        public int Save()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ModelManager));

            try
            {
                return Contexts.Sum(x => x.SaveChanges());
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception exception = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = $"{validationErrors.Entry.Entity}:{validationError.ErrorMessage}";
                        exception = new InvalidOperationException(message, exception);
                    }
                }
                throw exception;
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            if (Contexts != null)
            {
                foreach (var context in Contexts)
                {
                    context?.Dispose();
                }
                Contexts = null;
            }

            _disposed = true;
        }
    }
}
