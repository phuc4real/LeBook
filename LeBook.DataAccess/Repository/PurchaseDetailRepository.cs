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
    public class PurchaseDetailRepository : Repository<PurchaseDetail>, IPurchaseDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(PurchaseDetail detail)
        {
           _context.PurchaseDetails.Update(detail);
        }
    }
}