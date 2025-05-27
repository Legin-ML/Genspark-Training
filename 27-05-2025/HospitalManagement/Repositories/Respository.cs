
using System.Data;

public abstract class Repository<K, T> : IRepository<K, T> where T : class, IEntity<K>
{
    protected List<T> _contents = new List<T>();
    public T Add(T value)
    {
        var newId = GenerateId();
        var checkId = typeof(T).GetProperty("Id");

        if (checkId != null)
        {
            checkId.SetValue(value, newId);
        }

        if (_contents.Contains(value))
        {
            throw new DuplicateNameException("Already exists");
        }
        _contents.Add(value);
        return value;

    }

    public T Delete(K id)
    {
        var reqDelete = GetById(id);

        if (reqDelete == null)
        {
            throw new KeyNotFoundException("Not found");
        }

        _contents.Remove(reqDelete);
        return reqDelete;
    }

    public T Update(T value)
    {
        var reqUpdate = GetById((K)value.GetType().GetProperty("Id").GetValue(value));

        if (reqUpdate == null)
        {
            throw new KeyNotFoundException("Not found for updation");
        }

        var existing = _contents.IndexOf(reqUpdate);
        _contents[existing] = reqUpdate;
        return reqUpdate;
    }

    public abstract K GenerateId();

    public ICollection<T> GetAll()
    {
        return _contents.AsReadOnly();
    }

    public T GetById(K id)
    {
        var res = _contents.FirstOrDefault(e => e.Id.Equals(id));

        if (res == null)
        {
            throw new KeyNotFoundException("Not found");
        }

        return res;
    }
}
