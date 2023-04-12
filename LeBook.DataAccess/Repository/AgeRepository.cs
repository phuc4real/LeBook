using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using Microsoft.EntityFrameworkCore;
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