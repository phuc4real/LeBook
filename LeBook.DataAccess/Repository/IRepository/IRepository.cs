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
        T GetFirtOrDefault(Expression<Func<T, bool>> filter);

        IEnumerable<T> GetAll();

        bool Any(Expression<Func<T, bool>> filter);

        void Add(T item);

        void Remove(T item);

        void RemoveRange(IEnumerable<T> items);
    }
}
