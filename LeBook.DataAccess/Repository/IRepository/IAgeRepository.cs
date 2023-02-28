using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IAgeRepository : IRepository<Age>
    {
        void Update(Age age);

        void SoftDelete(Age age);

        void Restore(Age age);

        IEnumerable<Age> Get();

        IEnumerable<Age> GetDeleted();
    }
}
