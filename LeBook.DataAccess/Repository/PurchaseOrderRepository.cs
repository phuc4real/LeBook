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
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(PurchaseOrder purchase)
        {
           _context.PurchaseOrders.Update(purchase);
        }
    }
}