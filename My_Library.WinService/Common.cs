using My_Library.Core.Data;
using My_Library.Data;

namespace My_Library.WinService
{
    public static class Common
    {
        public static IUnitOfWork IUnitOfWork { get; set; }

        static Common()
        {
            if (IUnitOfWork == null) IUnitOfWork = new EfUnitOfWork(new EfDbContext());
        }
    }
}
