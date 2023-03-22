using LeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IUserAddressRepository : IRepository<UserAddress>
    {
        void Update(UserAddress address);

        IEnumerable<UserAddress> Get(string ApplicationUserId);
    }
    
}
