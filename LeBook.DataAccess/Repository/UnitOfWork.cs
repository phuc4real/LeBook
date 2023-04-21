using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Age = new AgeRepository(_context);
            Book = new BookRepository(_context);
            Order = new OrderRepository(_context);
            Price = new PriceRepository(_context);
            Company = new CompanyRepository(_context);
            Category = new CategoryRepository(_context);
            CoverType = new CoverTypeRepository(_context);
            UserAddress = new UserAddressRepository(_context);
            OrderDetail = new OrderDetailRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            PurchaseOrder = new PurchaseOrderRepository(_context);
            PurchaseDetail = new PurchaseDetailRepository(_context);
            Promotion = new PromotionRepository(_context);
            PromotionDetail = new PromotionDetailRepository(_context);
        }

        public IAgeRepository Age { get; private set; }

        public IBookRepository Book { get; private set; }

        public IOrderRepository Order { get; private set; }

        public IPriceReposittory Price { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public ICoverTypeRepository CoverType { get; private set; }

        public IUserAddressRepository UserAddress { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }

        public IPurchaseOrderRepository PurchaseOrder { get; private set; }

        public IPurchaseDetailRepository PurchaseDetail { get; private set; }

        public IPromotionRepository Promotion { get; private set; }

        public IPromotionDetailRepository PromotionDetail { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
