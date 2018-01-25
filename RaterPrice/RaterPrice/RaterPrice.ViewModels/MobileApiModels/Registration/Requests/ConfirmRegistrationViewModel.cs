using System.ComponentModel.DataAnnotations;

namespace RaterPrice.ViewModels.MobileApiModels.Registration
{
    public class ConfirmRegistrationViewModel
    {
        [DataType("int")]
        [Required(ErrorMessage = "Необходим идентификатор пользователя")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Код обязателен")]
        public string ConfirmationCode { get; set; }
    }
}
