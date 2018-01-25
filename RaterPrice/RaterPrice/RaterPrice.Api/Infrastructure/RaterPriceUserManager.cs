using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using RaterPrice.Persistence.Domain;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using RaterPrice.Services;
using Microsoft.Owin.Security.DataProtection;

namespace RaterPrice.Api.Infrastructure
{
    public class RaterPriceUserManager : UserManager<User, int>
    {
        public RaterPriceUserManager(IUserStore<User, int> store,
            IDataProtectionProvider dataProtectionProvider)
            : base(store)
        {
            UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            //PasswordHasher = new AppPasswordHasher();

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = false;
            //DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<DataModel.Entities.User, int>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});

            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            EmailService = new EmailService();
            //SmsService = new SmsService();
            //UserTokenProvider = new DataProtectorTokenProvider<DataModel.Entities.User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("RaterPriceIdentity"));

           
        }

        //public static RaterPriceUserManager Create(IUserStore<User, int> store,
        //    IDataProtectionProvider dataProtectionProvider)
        //{
          
        //}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user, string authenticationType)
        {
            var userIdentity = await CreateIdentityAsync(user, authenticationType);
            return userIdentity;
        }

        
    }
}