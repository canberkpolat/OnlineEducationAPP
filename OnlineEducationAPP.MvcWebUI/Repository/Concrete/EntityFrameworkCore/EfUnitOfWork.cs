using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Repository.Concrete.EntityFrameworkCore
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly OnlineEducationDbContext context;

        public EfUnitOfWork(OnlineEducationDbContext _context)
        {
            context = _context;
        }
        private ICourseRepository courses;
        private ICategoryRepository categories;
        private IStreamRepository streams;
        private IUserRepository users;
        private IEventRepository events;
        private IMessageRepository messages;


        public ICourseRepository Courses => courses ?? (courses = new EfCourseRepository(context));

        public ICategoryRepository Categories => categories ?? (categories = new EfCategoryRepository(context));

        public IStreamRepository Streams => streams ?? (streams = new EfStreamRepository(context));

        public IUserRepository Users => users ?? (users = new EfUserRepository(context));

        public IEventRepository Events => events ?? (events = new EfEventRepository(context));
        public IMessageRepository Messages => messages ?? (messages = new EfMessageRepository(context));



        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
