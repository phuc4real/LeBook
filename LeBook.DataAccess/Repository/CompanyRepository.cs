using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Company> Get()
        {
            IQueryable<Company> values = _context.Companies.Where(c => c.IsDeleted == false);
            return values.ToList();
        }

        public IEnumerable<Company> GetDeleted()
        {
            IQueryable<Company> values = _context.Companies.Where(c => c.IsDeleted == true);
            return values.ToList();
        }

        public void Restore(Company company)
        {
            company.IsDeleted = false;
            company.LastUpdatedAt = DateTime.Now;
        }

        public void SoftDelete(Company company)
        {
            company.IsDeleted = true;
            company.DeletedAt = DateTime.Now;
        }

        public void Update(Company company)
        {
            _context.Update(company);
        }
    }
}
