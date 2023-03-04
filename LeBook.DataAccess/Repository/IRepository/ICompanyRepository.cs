using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company company);

        void SoftDelete(Company company);

        void Restore(Company company);

        IEnumerable<Company> Get();

        IEnumerable<Company> GetDeleted();
    }
}
