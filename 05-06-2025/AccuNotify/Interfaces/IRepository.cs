namespace AccuNotify.Interfaces;

public interface IRepository<K, T> where T : class
{
    public Task<T> GetAsync(K id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> AddAsync(T item);
    public Task<T> UpdateAsync(K id, T item);
    public Task<T> DeleteAsync(K id);
}