namespace TrueFeedback.Interfaces;

public interface IRepository<K, T> where T : class
{
    Task<T> AddAsync(T item);
    Task<T> UpdateAsync(T item);
    Task<bool> DeleteAsync(T item);
    Task<T> GetAsync(K id);
    Task<IEnumerable<T>> GetAllAsync();
}