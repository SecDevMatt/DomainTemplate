using System.Collections.Generic;

namespace DomainTemplate.Interfaces
{
    public interface IReadOnlyRepository<out T>
    {
        IEnumerable<T> GetAll();
    }
}
