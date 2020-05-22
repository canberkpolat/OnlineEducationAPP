using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Repository.Concrete.EntityFrameworkCore
{
    public class EfUserRepository : EfGenericRepository<ApplicationUser>,IUserRepository
    {
        public EfUserRepository(OnlineEducationDbContext context) : base(context)
        {

        }
        public OnlineEducationDbContext DbContext => context as OnlineEducationDbContext;
    }
}
