using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository
{
    public class PriceRepository : Repository<Price>, IPriceReposittory
    {
        private readonly ApplicationDbContext _context;

        public PriceRepository(ApplicationDbContext context) :base(context)
        {
            _context = context;
        }

        public double GetItemPrice()
        {
            double ItemPrice = _context.Prices.OrderByDescending(p => p.Id).First().ItemPrice;
            return ItemPrice;
        }

        public Price SetPrice(string itemPrice, int BookId)
        {
            double _itemPrice = double.Parse(itemPrice);
            Price price = new Price();
            price.ItemPrice = _itemPrice;
            price.Date = DateTime.Now;
            price.BookId = BookId;
            return price;
        }
    }
}
