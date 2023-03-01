using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }

        ICoverTypeRepository CoverType { get; }

        IAgeRepository Age { get; }

        IPriceReposittory Price { get; }

        IBookRepository Book { get; }
        void Save();
    }
}
