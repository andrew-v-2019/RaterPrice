using Microsoft.AspNet.Identity;
using RaterPrice.Persistence.Domain;

namespace RaterPrice.Api.Infrastructure
{
    public class RoleManager : RoleManager<Role, int>
    {
        public RoleManager(IRoleStore<Role, int> roleStore)
        : base(roleStore)
        {

        }
    }
}