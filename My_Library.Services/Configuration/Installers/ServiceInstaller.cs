using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using My_Library.Core.Helpers;

namespace My_Library.Services.Configuration.Installers
{
    public class ServiceInstaller
      : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(Utility.AssemblyDirectory).FilterByName(a => a.Name.StartsWith("ISG")))
                                  .Where(x => x.Name.EndsWith("Service")) // && x.Name.Contains("SessionService")==false)
                                  .WithServiceDefaultInterfaces()
                                  .LifestyleTransient());

            //container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(Utility.AssemblyDirectory).FilterByName(a => a.Name.StartsWith("ISG")))
            //                     .Where(x => x.Name.Contains("SessionService"))
            //                     .WithServiceDefaultInterfaces()
            //                     .LifestyleCustom<PerSessionLifestyleManager>());
        }
    }
}
