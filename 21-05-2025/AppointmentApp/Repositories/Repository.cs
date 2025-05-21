

using AppointmentApp.Exceptions;
using AppointmentApp.Interfaces;

namespace AppointmentApp.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {

        public abstract K GenerateId();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);

        protected List<T> _items =  new List<T>();
        public T Add(T item)
        {
            var genId = GenerateId();
            var propExists = typeof(T).GetProperty("Id");

            if (propExists != null)
            {
                propExists.SetValue(item, genId);
            }

            if (_items.Contains(item))
            {
                throw new AppointmentExistsException("Appointment already exists");
            }

            _items.Add(item);
            return item;
        }
        public T Delete(K id)
        {
            var reqDeletion = GetById(id);

            if (reqDeletion == null)
            {
                throw new KeyNotFoundException("Appointment with given ID not found");
            }

            _items.Remove(reqDeletion);
            return reqDeletion;
        }

        public T Update(T item)
        {
            var reqUpdate = GetById((K)item.GetType().GetProperty("Id").GetValue(item));

            if ( reqUpdate == null)
            {
                throw new KeyNotFoundException("Appointment with given ID not found");
            }

            var index = _items.IndexOf(reqUpdate);
            _items[index] = reqUpdate;
            return reqUpdate;
        }
    }
}
