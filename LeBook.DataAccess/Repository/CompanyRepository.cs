using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using Microsoft.EntityFrameworkCore;
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
            _context.Companies.Update(company);
        }
    }
}
