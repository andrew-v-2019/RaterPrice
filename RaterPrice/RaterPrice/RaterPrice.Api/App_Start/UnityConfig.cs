using System;
using System.Configuration;
using RaterPrice.Api.Areas.MobileApi.Services;
using Microsoft.Practices.Unity;
using RaterPrice.Persistence.Domain;
using RaterPrice.SMS;
using RaterPrice.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using RaterPrice.Api.Infrastructure;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.Owin.Security.DataProtection;

namespace RaterPrice.Api
{
    /// <summary>
    ///     Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        ///     There is no need to register concrete types such as controllers or API controllers (unless you want to
        ///     change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            //container.LoadConfiguration();

            // container.RegisterType<RaterPriceContext>(new PerRequestLifetimeManager());

            container.RegisterType<IDataProtectionProvider, DpapiDataProtectionProvider>(new InjectionConstructor("ASP.NET Identity"));
                      // container.RegisterType<IDataProtectionProvider, DpapiDataProtectionProvider>(new PerRequestLifetimeManager());

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<RaterPriceUserManager>(new PerRequestLifetimeManager());
            container.RegisterType<SignInManager>(new PerRequestLifetimeManager());
            container.RegisterType<RoleManager>(new PerRequestLifetimeManager());
            container.RegisterType<RoleStore>(new PerRequestLifetimeManager());
            container.RegisterType< IUserStore <User, int>, UserStore >(new PerRequestLifetimeManager());
            container.RegisterType<RaterPriceContext>(new InjectionConstructor(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
            container.RegisterType<IQueryFacade, QueryFacade>(new PerRequestLifetimeManager());
            container.RegisterType<ICommandFacade, CommandFacade>(new PerRequestLifetimeManager());

            container.RegisterType<Areas.SiteApi.Services.IQueryFacade, Areas.SiteApi.Services.QueryFacade>(
                new PerRequestLifetimeManager());

            container.RegisterType<IRegistrationService, RegistrationService>(new PerRequestLifetimeManager());

            container.RegisterType<IUserService, UserService>(new PerRequestLifetimeManager());

            container.RegisterType<ISmsService, SmsService>(new PerRequestLifetimeManager());

            container.RegisterType<ISmsGateWay, SMSCGateWay>(new InjectionConstructor(ConfigurationManager.AppSettings["SMSServiceLogin"], ConfigurationManager.AppSettings["SMSServicePassword"]));
            //container.RegisterType<GoodsApiController>();
        }

        #region Unity Container

        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        ///     Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        #endregion
    }
}