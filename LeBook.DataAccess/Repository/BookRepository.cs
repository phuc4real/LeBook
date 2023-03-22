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
            return values.Include(c => c.Price).Include(c => c.Category).Include(c => c.CoverType).Include(c => c.Age);
        }

        public IEnumerable<Book> FindByAge(int AgeId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.AgeId == AgeId);
            values = AddProperty(values);
            return values.ToList();
        }

        public IEnumerable<Book> FindByCategory(int CategoryId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.CategoryId == CategoryId);
            values = AddProperty(values);
            return values.ToList();
        }

        public IEnumerable<Book> FindByCoverType(int CoverTypeId)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.CoverTypeId == CoverTypeId);
            values = AddProperty(values);
            return values.ToList();
        }

        public IEnumerable<Book> Get()
        {
            IQueryable<Book> values = _context.Books.Where(c => c.IsDeleted == false);
            values = AddProperty(values);
            return values.ToList();
        }

        public IEnumerable<Book> GetDeleted()
        {
            IQueryable<Book> values = _context.Books.Where(c => c.IsDeleted == true);
            values = AddProperty(values);
            return values.ToList();
        }

        public Book GetFirst(int? id)
        {
            IQueryable<Book> books = _context.Books.Where(c => c.Id == id);
            books = AddProperty(books);
            Book book = books.FirstOrDefault(c => c.Id == id);
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

        public IEnumerable<Book> Get10(string key)
        {
            IQueryable<Book> values = _context.Books.Where(c => c.IsDeleted == false).Include(c => c.Price);
            switch (key)
            {
                case "new":
                    values = values.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);
                    break;
                case "bestseller":
                    //loc hoa don
                    break;
                case "hotdeal":
                    //Loc khuyen mai
                    break;
                case "newmanga":
                    values = values.Where(c => c.Category.Id == 6);
                    values = values.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);
                    break;
                case "topmanga":
                    values = values.Where(c => c.Category.Id == 6);
                    //Loc top
                    break;
                case "newlightnovel":
                    values = values.Where(c => c.Category.Id == 22);
                    values = values.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);
                    break;
                case "cate1":
                    values = values.Where(c => c.Category.Id == 5);
                    break;
                case "cate2":
                    values = values.Where(c => c.Category.Id == 2);
                    break;
                case "cate3":
                    values = values.Where(c => c.Category.Id == 1);
                    break;
            }
            return values.Take(10).ToList();
        }
    }
}
