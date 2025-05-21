using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentApp.Exceptions
{
    public class CollectionEmptyException : Exception
    {
        public CollectionEmptyException()
        {
        }

        public CollectionEmptyException(string? message) : base(message)
        {

        }
    }
}
