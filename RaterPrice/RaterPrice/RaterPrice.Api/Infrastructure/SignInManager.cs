using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RaterPrice.Persistence.Domain;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RaterPrice.Api.Infrastructure
{
    public class SignInManager : SignInManager<User, int>
    {
        public SignInManager(RaterPriceUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }


        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((RaterPriceUserManager)UserManager);
        }
    }
}