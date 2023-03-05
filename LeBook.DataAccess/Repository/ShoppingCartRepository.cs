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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _context;

        public ShoppingCartRepository( ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int value)
        {
            shoppingCart.Count -= value;
            return shoppingCart.Count;
        }

        public IEnumerable<ShoppingCart> GetCart(string ApplicationUserId)
        {
            IQueryable<ShoppingCart> carts = _context.ShoppingCarts.Where(c => c.ApplicationUserId == ApplicationUserId);
            carts = carts.Include(c => c.Book).ThenInclude(b => b.Price);
            return carts.ToList();
        }

        public int IncrementCount(ShoppingCart shoppingCart, int value)
        {
            shoppingCart.Count += value;
            return shoppingCart.Count;
        }
    }
}
