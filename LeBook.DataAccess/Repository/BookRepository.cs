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

        private IQueryable<Book> AddProperty(IQueryable<Book> values)
        {
            return values.Include(c => c.Price).Include(c => c.Category1).Include(c => c.Category2).Include(c => c.CoverType).Include(c => c.Age);
        }

        public IEnumerable<Book> FindByAge(int AgeId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.AgeId == AgeId);
            values = AddProperty(values);
            return values.ToList();
        }

        public IEnumerable<Book> FindByCategory(int CategoryId, int level)
        {
            IQueryable<Book> values = _context.Books;
            if (level == 1)
            {
                values = values.Where(c => c.Category1Id == CategoryId);
            }
            else if (level == 2) 
            {
                values = values.Where(c => c.Category2Id == CategoryId);
            }
            values = AddProperty(values);
            return values.ToList();
        }

        public IEnumerable<Book> FindByCoverType(int CoverTypeId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.CoverTypeId == CoverTypeId);
            values = AddProperty(values);
            return values.ToList();
        }

        public Book GetFirst(int? id)
        {
            IQueryable<Book> books = _context.Books.Where(c => c.Id == id);
            books = AddProperty(books);
            Book book = books.First(c => c.Id == id);
            return book;
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
                _book.Category1Id = book.Category1Id;
                _book.Category2Id = book.Category2Id;
                _book.CoverTypeId = book.CoverTypeId;
                _book.AgeId = book.AgeId;
                _book.LastUpdatedAt = DateTime.Now;
                if (book.ImgUrl != null)
                {
                    _book.ImgUrl = book.ImgUrl;
                }
            }
        }

        public  IEnumerable<Book> Get10(string key)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.IsDeleted == false).Include(c => c.Price);
            switch (key)
            {
                case "new":
                    values = values.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);
                    break;
                case "bestseller":
                    var topSellingBooks = _context.Books.Where(book => !book.IsDeleted)
                        .Join(_context.OrderDetails, book => book.Id, details => details.BookId,(book, details) => new { Book = book, OrderDetail = details })
                        .AsEnumerable().GroupBy(x => x.Book).Select(bookGroup => new { Book = bookGroup.Key, TotalSales = bookGroup.Sum(x => x.OrderDetail.Quantity) })
                        .OrderByDescending(x => x.TotalSales).ToList();
                    var topsellerId = topSellingBooks.Select(x=> x.Book.Id);
                    values = values.Where(x => topsellerId.Contains(x.Id));
                    break;
                case "hotdeal":
                    //Loc khuyen mai
                    break;
                case "newmanga":
                    values = values.Where(c => c.Category1.Id == 6);
                    values = values.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);
                    break;
                case "topmanga":
                    values = values.Where(c => c.Category1.Id == 6);
                    //Loc top
                    break;
                case "newlightnovel":
                    values = values.Where(c => c.Category1.Id == 22);
                    values = values.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);
                    break;
                case "cate1":
                    values = values.Where(c => c.Category1.Id == 5);
                    break;
                case "cate2":
                    values = values.Where(c => c.Category1.Id == 2);
                    break;
                case "cate3":
                    values = values.Where(c => c.Category1.Id == 1);
                    break;
            }
            return values.Take(10).ToList();
        }

        public void UpdateBookQuantity(Book book, int quantity, bool cancel = false)
        {
            if (cancel)
            {
                book.InStock += quantity;
                book.Sold -= quantity;
            }
            else
            {
                book.InStock -= quantity;
                book.Sold += quantity;
            }
        }

        public void AddBookStock(int bookID, int quantity)
        {
            Book book = _context.Books.FirstOrDefault(c => c.Id == bookID);

            if (book == null)
            {
                book.InStock += quantity;
            }
        }
    }
}
