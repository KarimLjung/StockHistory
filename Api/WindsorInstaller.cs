using AL;
using Api.Controllers;
using BLL;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IYahooAPIService>().ImplementedBy<YahooAPIService>().LifestyleTransient());
            container.Register(Component.For<ITickerService>().ImplementedBy<TickerService>().LifestyleTransient());
            container.Register(Component.For<IStockRepository>().ImplementedBy<IStockRepository>().LifestyleTransient());
            container.Register(Component.For<ControllerBase>().ImplementedBy<StockInformationController>().LifestyleTransient());
        }
    }
}
