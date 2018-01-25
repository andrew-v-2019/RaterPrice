using RaterPrice.ViewModels.MobileApi.Models;
using RaterPrice.ViewModels.MobileApiModels.Registration;

namespace RaterPrice.Services
{
    public interface IRegistrationService
    {
        SendSmsResultViewModel RegisterUser(RegisterUserViewModel model);
        ConfirmationResultViewModel ConfirmUser(string code, int userId);
    }
}