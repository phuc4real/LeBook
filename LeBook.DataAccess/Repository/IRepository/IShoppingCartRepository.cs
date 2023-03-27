using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        IEnumerable<ShoppingCart> GetCart(string ApplicationUserId, bool buy);
        ShoppingCart GetCartById(int cartId);
        int IncrementCount(ShoppingCart shoppingCart, int value);
        int DecrementCount(ShoppingCart shoppingCart, int value);
        bool ToBuy(int cartId);
    }
}
