using System;

namespace DomainTemplate.Interfaces
{
    public interface IContext : IDisposable
    {
        int SaveChanges();
    }
}
