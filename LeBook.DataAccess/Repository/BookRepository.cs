using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository
{
    internal class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Book> FindByAge(int AgeId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.AgeId == AgeId);
            values = values.Include(c => c.Price).Include(c => c.Category).Include(c => c.CoverType).Include(c => c.Age);
            return values.ToList();
        }

        public IEnumerable<Book> FindByCategory(int CategoryId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.CategoryId == CategoryId);
            values = values.Include(c => c.Price).Include(c => c.Category).Include(c => c.CoverType).Include(c => c.Age);
            return values.ToList();
        }

        public IEnumerable<Book> FindByCoverType(int CoverTypeId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.CoverTypeId == CoverTypeId);
            values = values.Include(c => c.Price).Include(c => c.Category).Include(c => c.CoverType).Include(c => c.Age);
            return values.ToList();
        }

        public IEnumerable<Book> Get()
        {
            IQueryable<Book> values = _context.Books.Where(c => c.IsDeleted == false);
            values = values.Include(c => c.Price).Include(c => c.Category).Include(c => c.CoverType).Include(c => c.Age);
            return values.ToList();
        }

        public IEnumerable<Book> GetBestSeller()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetDeleted()
        {
            IQueryable<Book> values = _context.Books.Where(c => c.IsDeleted == true);
            values = values.Include(c => c.Price).Include(c => c.Category).Include(c => c.CoverType).Include(c => c.Age);
            return values.ToList();
        }

        public Book GetFirst(int? id)
        {
            IQueryable<Book> books = _context.Books.Where(c => c.Id == id);
            books = books.Include(c => c.Price).Include(c => c.Category).Include(c => c.CoverType).Include(c => c.Age);
            Book book = books.FirstOrDefault(c => c.Id == id);
            return book;
        }

        public IEnumerable<Book> GetHotDeal()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetNewBook()
        {
            IQueryable<Book> values = _context.Books.Where(c => c.IsDeleted == false);
            values = values.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay).Take(10);
            values = values.Include(c => c.Price);
            return values.ToList();
        }

        public void Restore(Book book)
        {
            book.IsDeleted = false;
            book.LastUpdatedAt = DateTime.Now;
        }

        public void SoftDelete(Book book)
        {
            book.IsDeleted = true;
            book.DeletedAt = DateTime.Now;
        }

        public void Update(Book book)
        {
            var _book = _context.Books.FirstOrDefault(c => c.Id == book.Id);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.ISBN = book.ISBN;
                _book.Description = book.Description;
                _book.Author = book.Author;
                _book.Publisher = book.Publisher;
                _book.PublicationDate = book.PublicationDate;
                _book.InStock = book.InStock;
                _book.Sold = book.Sold;
                _book.CategoryId = book.CategoryId;
                _book.CoverTypeId = book.CoverTypeId;
                _book.AgeId = book.AgeId;
                _book.LastUpdatedAt = DateTime.Now;
                if (book.ImgUrl != null)
                {
                    _book.ImgUrl = book.ImgUrl;
                }
            }
        }
    }
}
