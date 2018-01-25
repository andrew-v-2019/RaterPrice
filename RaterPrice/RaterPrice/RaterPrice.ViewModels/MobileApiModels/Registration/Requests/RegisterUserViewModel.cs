using System.ComponentModel.DataAnnotations;

namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class RegisterUserViewModel
    {


        [StringLength(12, MinimumLength = 10, ErrorMessage = "Неправильный формат номера телефона")]
        [Required(ErrorMessage = "Телефон обязателен")]
        public string Phone { get; set; }
        public string Login { get; set; } 
        public string Password { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}