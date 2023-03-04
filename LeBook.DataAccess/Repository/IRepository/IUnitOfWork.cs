using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {


        IAgeRepository Age { get; }

        IBookRepository Book { get; }

        IPriceReposittory Price { get; }

        ICompanyRepository Company { get; }

        ICategoryRepository Category { get; }

        ICoverTypeRepository CoverType { get; }

        IShoppingCartRepository ShoppingCart { get; }

        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}
