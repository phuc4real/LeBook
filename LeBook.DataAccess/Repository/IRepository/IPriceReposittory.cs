using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IPriceReposittory : IRepository<Price>
    {
        Price SetPrice(string itemPrice, int BookId);

        double GetItemPrice();
    }
}
