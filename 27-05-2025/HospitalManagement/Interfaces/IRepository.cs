public interface IRepository<K, T> where T : class
{

    T Add(T value);
    T Update(T value);
    T Delete(K id);
    T GetById(K id);

    ICollection<T> GetAll();

    K GenerateId();
}