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

        public IEnumerable<ShoppingCart> GetCart(string ApplicationUserId, bool buy)
        {
            IQueryable<ShoppingCart> carts = _context.ShoppingCarts.Where(c => c.ApplicationUserId == ApplicationUserId);
            if (buy) { carts = carts.Where(carts => carts.toBuy == true ); }
            carts = carts.Include(c => c.Book).ThenInclude(b => b.Price);
            return carts.ToList();
        }

        public ShoppingCart GetCartById(int cartId)
        {
            ShoppingCart cart = _context.ShoppingCarts.FirstOrDefault(c => c.Id == cartId);
            return cart;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int value)
        {
            shoppingCart.Count += value;
            return shoppingCart.Count;
        }

        public bool ToBuy(int cartId)
        {
            ShoppingCart cart = _context.ShoppingCarts.FirstOrDefault(c => c.Id == cartId);
            cart.toBuy = !cart.toBuy;

            return cart.toBuy;
        }
    }
}
