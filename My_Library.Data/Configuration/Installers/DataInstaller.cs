using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using My_Library.Core.Data;

namespace My_Library.Data.Configuration.Installers
{
    public class DataInstaller
       : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<DbContext>()
                                        .ImplementedBy<EfDbContext>()
                                        .LifestylePerWebRequest()
                                        .OnCreate((kernel, instance) => LogForCreatedObject(kernel.Resolve<ILogger>(), instance))
                                        .OnDestroy((kernel, instance) => LogForDisposedObject(kernel.Resolve<ILogger>(), instance)),
                               Component.For<IUnitOfWork>()
                                        .ImplementedBy<EfUnitOfWork>()
                                        .LifestylePerWebRequest()
                                         .OnCreate((kernel, instance) => LogForCreatedObject(kernel.Resolve<ILogger>(), instance))
                                        .OnDestroy((kernel, instance) => LogForDisposedObject(kernel.Resolve<ILogger>(), instance)),
                               Component.For(typeof(IRepository<>))
                                        .ImplementedBy(typeof(EfRepository<>))
                                        .LifestylePerWebRequest()
                                         .OnCreate((kernel, instance) => LogForCreatedObject(kernel.Resolve<ILogger>(), instance))
                                        .OnDestroy((kernel, instance) => LogForDisposedObject(kernel.Resolve<ILogger>(), instance)));
        }

        private void LogForCreatedObject(ILogger logger, object o)
        {
            var childLogger = logger.CreateChildLogger(GetType().FullName);
            if (childLogger.IsDebugEnabled)
                childLogger.DebugFormat("'{0}' created", o.GetType().FullName);
        }

        private void LogForDisposedObject(ILogger logger, object o)
        {
            var childLogger = logger.CreateChildLogger(GetType().FullName);
            if (childLogger.IsDebugEnabled)
                childLogger.DebugFormat("'{0}' disposed", o.GetType().FullName);
        }
    }
}
