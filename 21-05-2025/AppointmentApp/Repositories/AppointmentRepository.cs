using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentApp.Exceptions;
using AppointmentApp.Models;

namespace AppointmentApp.Repositories
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        public AppointmentRepository() { }
        public override int GenerateId()
        {
            if (_items == null || _items.Count == 0)
            {
                return 101;
            }
            else
            {
                return _items.Max(e => e.Id + 1);
            }
        }

        public override ICollection<Appointment> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new CollectionEmptyException("No Records found");
            }

            return _items;

        }

        public override Appointment GetById(int id)
        {
            var reqById = _items.FirstOrDefault(e => e.Id == id);

            if(reqById == null)
            {
                throw new KeyNotFoundException("No Appointments found with given ID");
            }

            return reqById;
        }
    }
}
