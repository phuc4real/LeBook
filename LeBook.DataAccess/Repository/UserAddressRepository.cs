using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository
{
    public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
    {

        private ApplicationDbContext _context;

        public UserAddressRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<UserAddress> Get(string ApplicationUserId)
        {
            IQueryable<UserAddress> userAddresses = _context.UserAddresses.Where(c => c.UserId == ApplicationUserId);
            userAddresses = userAddresses.Include(c => c.User);
            return userAddresses.ToList();
        }

        public void Update(UserAddress address)
        {
            _context.UserAddresses.Update(address);
        }
    }
}
