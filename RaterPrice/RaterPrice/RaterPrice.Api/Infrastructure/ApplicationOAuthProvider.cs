using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using RaterPrice.Persistence.Domain;
using System.Security.Claims;
using RaterPrice.Services;
using System.Web;

namespace RaterPrice.Api.Infrastructure
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private RaterPriceUserManager _userManager;

        public ApplicationOAuthProvider(string publicClientId, RaterPriceUserManager userManager)
        {
             _userManager = userManager;

            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //var _userManager = context.OwinContext.GetUserManager<RaterPriceUserManager>();
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            User user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Invalid username or password.");
                return;
            }
            if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "Email не подтверждён");
                return;
            }

            ClaimsIdentity oAuthIdentity = await _userManager.GenerateUserIdentityAsync(user, OAuthDefaults.AuthenticationType);
            AuthenticationProperties properties = new AuthenticationProperties();
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }
    }
}