﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Repository.Abstract
{
    public interface IGenericRepository<T> where T:class
    {
        T Get(int id);
        void Add(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Delete(T entity);
        



    }
}
