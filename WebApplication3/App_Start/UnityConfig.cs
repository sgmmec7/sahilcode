using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using WebApplication3.Interfaces;
using WebApplication3.Repository;

namespace WebApplication3
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            container.RegisterType<IDataRepository, DataRepository>();
        }
    }
}