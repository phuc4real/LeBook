using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository
{
    internal class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public CoverTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<CoverType> Get()
        {
            IQueryable<CoverType> values = _context.CoverTypes.Where(c => c.IsDeleted == false);
            return values.ToList();
        }

        public IEnumerable<CoverType> GetDeleted()
        {
            IQueryable<CoverType> values = _context.CoverTypes.Where(c => c.IsDeleted == true);
            return values.ToList();
        }

        public void Restore(CoverType coverType)
        {
            coverType.IsDeleted = false;
            coverType.LastUpdatedAt = DateTime.Now;
        }

        public void SoftDelete(CoverType coverType)
        {
            coverType.IsDeleted = true;
            coverType.DeletedAt = DateTime.Now;
        }

        public void Update(CoverType coverType)
        {
            _context.Update(coverType);
        }
    }
}
