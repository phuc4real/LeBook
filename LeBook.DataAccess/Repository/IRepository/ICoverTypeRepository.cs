using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository :IRepository<CoverType>
    {
        void Update(CoverType coverType);

        void SoftDelete(CoverType coverType);

        void Restore(CoverType coverType);

        IEnumerable<CoverType> Get();

        IEnumerable<CoverType> GetDeleted();
    }
}
