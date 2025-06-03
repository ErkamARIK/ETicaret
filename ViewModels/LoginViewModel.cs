using System.ComponentModel.DataAnnotations;

namespace ETicaret.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta gereklidir")]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Şifre gereklidir")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        public bool RememberMe { get; set; }
    }
}
