﻿using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        IEnumerable<ShoppingCart> GetCart(string ApplicationUserId);
        int IncrementCount(ShoppingCart shoppingCart, int value);
        int DecrementCount(ShoppingCart shoppingCart, int value);
    }
}