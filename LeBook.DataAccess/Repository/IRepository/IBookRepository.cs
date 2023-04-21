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

        void UpdateBookQuantity(Book book,int quantity, bool cancel =false);

        void AddBookStock(Book book, int quantity);

        Book GetFirst(int? id);

        IEnumerable<Book> Get10(String key);

        IEnumerable<Book> FindByCategory(int CategoryId, int level);

        IEnumerable<Book> FindByCoverType(int CoverTypeId);

        IEnumerable<Book> FindByAge(int AgeId);
    }
}
