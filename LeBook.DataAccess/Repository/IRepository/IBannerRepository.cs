using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IBannerRepository : IRepository<Banner>
    {
        void Update(Banner banner);

        void SoftDelete(Banner banner);

        void Restore(Banner banner);

        IEnumerable<Banner> Get();

        IEnumerable<Banner> GetDeleted();
    }
}
