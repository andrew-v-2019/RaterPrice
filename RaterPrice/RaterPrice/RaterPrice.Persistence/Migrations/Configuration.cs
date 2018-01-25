namespace RaterPrice.Persistence.Migrations
{
    using Domain;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Security.Claims;

    public sealed class Configuration : DbMigrationsConfiguration<RaterPriceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }


        protected override void Seed(RaterPriceContext context)
        {
            SmsSender s = new SmsSender()
            {
                Name = "RaterPrice"
            };
            context.SmsSenders.Add(s);
            //var role = CreateAdmins(context);
            //CreateAdmin(context, role);
        }



    }
}
