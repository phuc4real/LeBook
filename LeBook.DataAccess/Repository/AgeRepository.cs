using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository
{
    public class AgeRepository : Repository<Age>, IAgeRepository
    {
        private readonly ApplicationDbContext _context;
        public AgeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void SoftDelete(Age age)
        {
            age.IsDeleted = true;
            age.DeletedAt = DateTime.Now;
        }

        public IEnumerable<Age> Get()
        {
            IQueryable<Age> values = _context.Ages.Where(c => c.IsDeleted == false);
            return values.ToList();
        }

        public IEnumerable<Age> GetDeleted()
        {
            IQueryable<Age> values = _context.Ages.Where(c => c.IsDeleted == true);
            return values.ToList();
        }

        public void Restore(Age age)
        {
            age.IsDeleted = false;
            age.LastUpdatedAt = DateTime.Now;
        }

        public void Update(Age age)
        {
            _context.Ages.Update(age);
        }
    }
}