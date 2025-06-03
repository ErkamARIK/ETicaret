using System.ComponentModel.DataAnnotations;

namespace ETicaret.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; } = default!;

        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }
    }
}