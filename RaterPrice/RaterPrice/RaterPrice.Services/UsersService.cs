using RaterPrice.Persistence.Domain;
using RaterPrice.ViewModels.MobileApi.Models;
using System.Linq;

namespace RaterPrice.Services
{


    public class UserService : IUserService
    {
        private readonly RaterPriceContext _raterPriceContext;

        public UserService(RaterPriceContext raterPriceContext)
        {
            _raterPriceContext = raterPriceContext;
        }


        public User CreateSimpleUser(RegisterUserViewModel viewModel)
        {
            var user = MapUserToDomainModel(viewModel);


            _raterPriceContext.Users.Add(user);
            //_raterPriceContext.SaveChanges();
            //    var role = new UserRole
            //    {
            //        UserId = user.Id,
            //        RoleId = _raterPriceContext.UserRoles.Where(r => r.Name.Equals("User")).Select(r => r.Id).First()
            //    };
            //_raterPriceContext.UserInRoles.Add(role);
            _raterPriceContext.SaveChanges();
            
            return user;
        }

        private User MapUserToDomainModel(RegisterUserViewModel user)
        {
            var domainUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.Login,
                Password = user.Password,
                PhoneNumber = user.Phone
            };
            return domainUser;
        }

        public bool CheckPhone(string phone)
        {
            
                if (_raterPriceContext.Users.Any(u => u.PhoneNumber.Equals(phone)))
                {
                    return false;
                }
                return true;
            
        }

    }
}
