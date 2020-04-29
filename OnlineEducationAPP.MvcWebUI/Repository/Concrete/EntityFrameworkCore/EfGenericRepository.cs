using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Repository.Concrete.EntityFrameworkCore
{
    public class EfGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext context;

        public EfGenericRepository(DbContext _context)
        {
            context = _context;
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

        public T Get(int id)
        {
            return context.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }
    }
}
