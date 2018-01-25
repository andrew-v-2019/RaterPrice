using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RaterPrice.Persistence.Domain;
using System;

namespace RaterPrice.Api.Infrastructure
{
    public class RoleStore : RoleStore<Role, int, UserRole>,
        IQueryableRoleStore<Role, int>,
        IRoleStore<Role, int>, IDisposable
    {
        public RoleStore()
            : base(new RaterPriceContext())
        {
            base.DisposeContext = true;
        }


        public RoleStore(RaterPriceContext context)
            : base(context)
        {

        }
    }
}