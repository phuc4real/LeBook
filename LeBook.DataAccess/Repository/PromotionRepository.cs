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

        public void Update(Promotion promotion)
        {
            _context.Promotion.Update(promotion);
        }
    }
}