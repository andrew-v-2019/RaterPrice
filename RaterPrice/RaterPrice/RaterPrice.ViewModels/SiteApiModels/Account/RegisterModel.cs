using System.ComponentModel.DataAnnotations;

namespace RaterPrice.ViewModels.SiteApiModels.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string RegisterEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public  string RegisterPassword { get; set; }

        [Required(ErrorMessage = "You should confirm the Password.")]
        public string RegisterPassword2 { get; set; }

    }
}
