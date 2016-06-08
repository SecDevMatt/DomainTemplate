using System;
using System.Collections.Generic;

namespace DomainTemplate.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Save();
        IContext AssignContext<T>() where T : class, IContext;
        ICollection<IContext> Contexts { get; }
    }
}
