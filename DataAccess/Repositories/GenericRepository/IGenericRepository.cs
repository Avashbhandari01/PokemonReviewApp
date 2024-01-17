namespace DataAccess.Repositories.GenericRepository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<TEntity> GetByIDAsync<TEntity>(object id) where TEntity : class;

    Task<bool> AddAsync(T entity);

    Task UpdateAsync<TEntity>(TEntity entityToUpdate) where TEntity : class;

    void Delete<TEntity>(object id) where TEntity : class;
}
