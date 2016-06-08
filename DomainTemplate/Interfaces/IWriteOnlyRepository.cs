namespace DomainTemplate.Interfaces
{
    public interface IWriteOnlyRepository<in T>
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
