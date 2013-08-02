using System.Collections.Generic;
using System.Linq;

namespace My_Library.Core.Services
{
    public interface IDefaultServiceOperation<T>
    {
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void DeleteById(int id);

        T GetById(int id);
        IList<T> GetAll();
        IQueryable<T> GetAllQueryable(bool include);
    }
}
