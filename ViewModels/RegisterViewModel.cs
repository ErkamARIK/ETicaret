using System.ComponentModel.DataAnnotations;

namespace ETicaret.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-posta gereklidir")]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Şifre gereklidir")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Şifre tekrarı gereklidir")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
