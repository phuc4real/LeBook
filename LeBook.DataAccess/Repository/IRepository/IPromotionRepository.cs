using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        void Update(Promotion promotion);
    }
}
