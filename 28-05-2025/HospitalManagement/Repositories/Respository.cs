using HospitalManagement.Contexts;
using HospitalManagement.Interfaces;

namespace HospitalManagement.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly HospitalContext _hospitalContext;

        public Repository(HospitalContext hospitalContext)
        {
            _hospitalContext = hospitalContext;
        }
        public async Task<T> Add(T item)
        {
            _hospitalContext.Add(item);
            await _hospitalContext.SaveChangesAsync();
            return item;
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _hospitalContext.Remove(item);
                await _hospitalContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

        public abstract Task<T> Get(K key);


        public abstract Task<IEnumerable<T>> GetAll();


        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _hospitalContext.Entry(myItem).CurrentValues.SetValues(item);
                await _hospitalContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }
    }
}

