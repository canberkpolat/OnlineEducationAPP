using OnlineEducationAPP.MvcWebUI.Entity;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Repository.Concrete.EntityFrameworkCore
{
    public class EfStreamRepository : EfGenericRepository<Stream>, IStreamRepository
    {
        public EfStreamRepository(OnlineEducationDbContext context) : base(context)
        {

        }
        public OnlineEducationDbContext DbContext => context as OnlineEducationDbContext;
    }
}
