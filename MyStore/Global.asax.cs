using FluentValidation.Mvc;
using MyStore.Business;
using MyStore.DataAccess;
using MyStore.DataAccess.Interface;
using MyStore.Framework;
using MyStore.Infrastructure;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            AutoMapperStartup.Initialize();
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new MyStoreValidatorFactory()));
            Database.SetInitializer<EfContext>(null);
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<IDbContext>()
                .To<EfContext>().InRequestScope();

            kernel.Bind<IUserService>()
                .To<UserService>().InRequestScope();

            return kernel;
        }
    }
}