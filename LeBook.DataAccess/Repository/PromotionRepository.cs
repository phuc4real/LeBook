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
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        private readonly ApplicationDbContext _context;
        public PromotionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Restore(Promotion promotion)
        {
            promotion.IsDeleted = false;
            promotion.LastUpdatedAt = DateTime.Now;
        }

        public void SoftDelete(Promotion promotion)
        {
            promotion.IsDeleted = true;
            promotion.DeletedAt = DateTime.Now;
        }

        public void Update(Promotion promotion)
        {
            _context.Promotion.Update(promotion);
        }
    }
}