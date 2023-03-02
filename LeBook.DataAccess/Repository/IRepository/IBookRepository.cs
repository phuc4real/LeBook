using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book book);

        void SoftDelete(Book book);

        void Restore(Book book);

        Book GetFirst(int? id);

        IEnumerable<Book> Get();

        IEnumerable<Book> GetDeleted();

        IEnumerable<Book> FindByCategory(int CategoryId);

        IEnumerable<Book> FindByCoverType(int CoverTypeId);

        IEnumerable<Book> FindByAge(int AgeId);
    }
}
