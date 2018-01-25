using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RaterPrice.Persistence.Domain;

namespace RaterPrice.Api.Infrastructure
{
    public class UserStore : UserStore<User,Role,int, UserLogin, UserRole, UserClaim>,IUserStore<User, int>
    {
        public UserStore() : this(new RaterPriceContext())
        {
            base.DisposeContext = true;
        }

        public UserStore(RaterPriceContext context) : base(context)
        {
        }
    }
}