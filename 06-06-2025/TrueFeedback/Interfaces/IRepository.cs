namespace TrueFeedback.Interfaces;

public interface IRepository<K, T> where T : class
{
    Task<T> AddAsync(T item);
    Task<T> UpdateAsync(K id, T item);
    Task<bool> DeleteAsync(K id);
    Task<T> GetAsync(K id);
    Task<IEnumerable<T>> GetAllAsync();
}