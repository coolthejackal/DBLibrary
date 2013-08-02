using System;
using System.Data.Entity;
using Castle.Windsor;
using My_Library.Core.Data;
using My_Library.Core.Domain;
using My_Library.Core.Services;
using My_Library.Data;
using My_Library.Services;
using My_Library.WinService;

namespace My_Library.Test
{
    public class Program
    {
        //public static IUnitOfWork unitOfWork = new EfUnitOfWork(new EfDbContext());

        static void Main(string[] args)
        {
            try
            {
                Departments departments = new Departments();
                Department d = new Department {Name = "asd"};

                departments.AddDepartment(d);
                Common.IUnitOfWork.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                
            }

            Console.ReadLine();
        }

        public class Departments
        {
            private readonly IDepartmentService _departmentService;

            public Departments()
            {
                if (_departmentService == null) 
                    _departmentService = new DepartmentService(new EfRepository<Department>(Common.IUnitOfWork));
            }

            public void AddDepartment(Department department)
            {
                _departmentService.Add(department);
            }
        }
    }
}
