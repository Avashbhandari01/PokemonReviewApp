using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;

namespace DataAccess.Repositories.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
{
    private readonly DataContext _context;

    public GenericRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        var result = await _context.SaveChangesAsync();

        if (result > 0) return true;
        else return false;
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task<TEntity> GetByIDAsync<TEntity>(object id) where TEntity : class
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async virtual Task UpdateAsync<TEntity>(TEntity entityToUpdate) where TEntity : class
    {
        if (entityToUpdate == null)
        {
            throw new ArgumentNullException("entity");
        }

        _context.Entry(entityToUpdate).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual void Delete<TEntity>(object id) where TEntity : class
    {
        TEntity entityToDelete = _context.Set<TEntity>().Find(id);
        _context.Remove(entityToDelete);
        _context.SaveChanges();
    }
}
