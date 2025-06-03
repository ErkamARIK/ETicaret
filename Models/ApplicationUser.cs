using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
