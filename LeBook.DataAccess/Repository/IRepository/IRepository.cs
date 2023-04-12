using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T FirstOrDefault(Expression<Func<T, bool>> filter);

        IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        bool Any(Expression<Func<T, bool>> filter);

        void Add(T item);

        void Remove(T item);

        void RemoveRange(IEnumerable<T> items);
    }
}
