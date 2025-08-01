namespace ShopMigrationAPI.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T item);
    void Update(T item);
    void Delete(int id);
    void Save();
}