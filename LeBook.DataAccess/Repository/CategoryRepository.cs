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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void SoftDelete(Category category)
        {
            category.IsDeleted = true;
            category.DeletedAt = DateTime.Now;
        }

        public void Restore(Category category)
        {
            category.IsDeleted = false;
            category.LastUpdatedAt = DateTime.Now;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}