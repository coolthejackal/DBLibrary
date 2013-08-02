using System.Collections.Generic;
using System.Linq;
using My_Library.Core.Data;
using My_Library.Core.Domain;
using My_Library.Core.Services;

namespace My_Library.Services
{
    public class DepartmentService
        : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentService(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void Add(Department item)
        {
            _departmentRepository.Add(item);
        }

        public void Update(Department item)
        {
            _departmentRepository.Update(item);
        }

        public void Delete(Department item)
        {
            _departmentRepository.Delete(item);
        }

        public void DeleteById(int id)
        {
            var d = GetById(id);
            if(d != null) _departmentRepository.Delete(d);
        }

        public Department GetById(int id)
        {
            return GetAllQueryable(true).FirstOrDefault(f => f.Id == id);
        }

        public IList<Department> GetAll()
        {
            return GetAllQueryable(false).ToList();
        }

        public IQueryable<Department> GetAllQueryable(bool include)
        {
            return include ? _departmentRepository.GetAllIncluding(d => d.Employees) : _departmentRepository.GetAll();
        }
    }
}
