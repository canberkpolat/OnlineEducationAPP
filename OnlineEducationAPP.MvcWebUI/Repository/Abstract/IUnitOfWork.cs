using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Repository.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        ICourseRepository Courses { get; }
        IStreamRepository Streams { get; }
        IUserRepository Users { get; }
        IEventRepository Events { get; }
        IMessageRepository Messages { get; }
        

        int SaveChanges();
    }
}
