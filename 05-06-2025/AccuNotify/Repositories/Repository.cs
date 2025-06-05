using AccuNotify.Contexts;
using AccuNotify.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccuNotify.Repositories;

public abstract class Repository<K, T> : IRepository<K, T> where T : class
{
    protected readonly AccuNotifyContext _context;
    protected readonly DbSet<T> _set;

    public Repository(AccuNotifyContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public async Task<T> AddAsync(T item)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _set.AddAsync(item);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return item;
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Failed to add item to the database.", ex);
        }
    }

    public async Task<T> UpdateAsync(K key, T item)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existing = await _set.FindAsync(key);
            if (existing == null)
                throw new KeyNotFoundException($"Item with key '{key}' not found for update.");

            _context.Entry(existing).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return existing;
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException($"Failed to update item with key '{key}'.", ex);

        }
    }

    public async Task<T> GetAsync(K key)
    {
        try
        {
            var item = await _set.FindAsync(key);
            if (item == null)
                throw new KeyNotFoundException($"Item with key '{key}' not found.");
            return item;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve item with key '{key}'.", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _set.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve all items of type {typeof(T).Name}.", ex);
        }
    }

    public async Task<T> DeleteAsync(K key)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existing = await _set.FindAsync(key);
            if (existing == null)
                throw new KeyNotFoundException($"Item with key '{key}' not found for deletion.");

            _set.Remove(existing);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return existing;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException($"Failed to delete item with key '{key}'.", ex);
        }
    }
}