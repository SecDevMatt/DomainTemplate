namespace DomainTemplate.Interfaces
{
    public interface IRepository<T> : IReadOnlyRepository<T>, IWriteOnlyRepository<T> where T : class
    {

    }
}
