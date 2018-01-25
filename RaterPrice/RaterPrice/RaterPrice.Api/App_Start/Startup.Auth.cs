using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RaterPrice.Api.Infrastructure;
using RaterPrice.Persistence.Domain;
using System.Web;
using System.Web.Mvc;

namespace RaterPrice.Api
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // Дополнительные сведения о настройке аутентификации см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            var manager = DependencyResolver.Current.GetService<RaterPriceUserManager>();//app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<RaterPriceUserManager>());

         PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
             
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, manager),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(10),
                AllowInsecureHttp = true,
                AuthenticationType = "password",
                
            };

            app.UseOAuthBearerTokens(OAuthOptions);


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                         SecurityStampValidator.OnValidateIdentity<RaterPriceUserManager, User, int>(
                             TimeSpan.FromMinutes(30),
                             (_manager, _usr) => _usr.GenerateUserIdentityAsync(_manager),
                             (_claim) => int.Parse(_claim.GetUserId())
                         )
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information 
            // when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(30));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will 
            // be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}