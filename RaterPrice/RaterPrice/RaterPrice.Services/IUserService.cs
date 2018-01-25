using RaterPrice.Persistence.Domain;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Services
{
    public interface IUserService
    {
        User CreateSimpleUser(RegisterUserViewModel viewModel);
        bool CheckPhone(string phone);
    }
}
