using LegitBank.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LegitBank.Repositories;

public abstract class Repository<K, T> : IRepository<K,T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _set;

    public Repository(DbContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }
    public async Task<T> Add(T item)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return item;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
        
    }

    public async Task<T> Update(K key, T item)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var existing = await _set.FindAsync(key);
            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return existing;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<T> Get(K key)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            return await _set.FindAsync(key);

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<T> Delete(K key)
    {
        using var  transaction = _context.Database.BeginTransaction();

        try
        {
            
            var existing = await _set.FindAsync(key);
            if (existing == null)
                return null;
            _set.Remove(existing);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return existing;

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }
}
