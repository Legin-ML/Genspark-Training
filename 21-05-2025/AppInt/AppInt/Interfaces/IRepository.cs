﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppInt.Models;

namespace AppInt.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        T Add(T item);
        T Update(T item);
        T Delete(K id);
        T GetById(K id);
        ICollection<T> GetAll();
        //Employee GetById(int id);
    }
}
