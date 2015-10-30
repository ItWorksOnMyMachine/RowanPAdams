using System;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using RowanAdams.Entities.Models;
using RowanAdams.Services;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using RowanAdams.Controllers;
using RowanAdams.Entities;
using RowanAdams.Models;

namespace RowanAdams.App_Start
{
	/// <summary>
	/// Specifies the Unity configuration for the main container.
	/// </summary>
	public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
			// NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
			// container.LoadConfiguration();

			// TODO: Register your types here
			//container
				//.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager())
				//.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager())
				//.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager())
				//.RegisterType<AccountController>(new InjectionConstructor());


			container
				.RegisterInstance<ITimeService>(new TimeService())
				.RegisterInstance<IConfigurationService>(new ConfigurationService())
				.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new PerRequestLifetimeManager(), new InjectionConstructor(new ApplicationDbContext()))
				.RegisterType<AccountController>(new InjectionConstructor())
				.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication))
		        .RegisterType<IDataContextAsync, EntitiesContext>(new PerRequestLifetimeManager(), new InjectionConstructor())
		        .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
		        .RegisterType<IRepositoryAsync<Chore>, Repository<Chore>>()
		        .RegisterType<IRepositoryAsync<LogEntry>, Repository<LogEntry>>()
		        .RegisterType<IChoreService, ChoreService>()
				.RegisterType<ILogEntryService, LogEntryService>();
        }
	}
}
