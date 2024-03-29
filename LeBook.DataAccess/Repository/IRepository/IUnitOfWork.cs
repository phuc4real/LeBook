﻿using System;
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

        IOrderRepository Order { get; }

        IPriceReposittory Price { get; }

        ICompanyRepository Company { get; }

        ICategoryRepository Category { get; }

        ICoverTypeRepository CoverType { get; }

        IUserAddressRepository UserAddress { get; }

        IOrderDetailRepository OrderDetail { get; }

        IShoppingCartRepository ShoppingCart { get; }

        IPurchaseOrderRepository PurchaseOrder { get; }

        IPurchaseDetailRepository PurchaseDetail { get; }

        IPromotionRepository Promotion { get; }

        IPromotionDetailRepository PromotionDetail { get; }

        void Save();
    }
}
