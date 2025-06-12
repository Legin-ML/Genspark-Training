
using Microsoft.EntityFrameworkCore;
using TrueFeedback.Contexts;
using TrueFeedback.Interfaces;
using TrueFeedback.Models;

namespace TrueFeedback.Repositories;

public abstract class Repository<K, T> : IRepository<K, T> where T : class, IEntity
{
    private readonly TrueFeedbackContext _context;
    private readonly DbSet<T> _set;
    private readonly ILogger<T> _logger;
    
    protected Repository(TrueFeedbackContext context, ILogger<T> logger)
    {
        _context = context;
        _set = _context.Set<T>();
        _logger = logger;
    }
    public async Task<T> AddAsync(T item)
    {
        await _set.AddAsync(item);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Item with id {item.Id} added successfully");
        return item;
    }

    public async Task<T> UpdateAsync(K id,T item)
    {
        var oldValue = await GetAsync(id);
        if (oldValue == null)
        {
            throw new KeyNotFoundException($"Item with id {id} not found");       
        }
        _context.Entry(oldValue).CurrentValues.SetValues(item);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Item with id {id} updated successfully");
        return item;
    }

    public async Task<bool> DeleteAsync(K id)
    {
        var value = await GetAsync(id);
        if (value == null)
        {
            throw new KeyNotFoundException($"Item with id {id} not found");
        }
        _set.Remove(value);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Item with id {id} deleted successfully");
        return true;
    }

    public async Task<T> GetAsync(K id)
    {
        var value = await _set.FindAsync(id);
        if (value == null)
        {
            throw new KeyNotFoundException($"Item with id {id} not found");
        }
        _logger.LogInformation($"Item with id {id} fetched successfully");
        return value;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.LogInformation($"All Items fetched successfully");
        return await _set.ToListAsync();
    }
}