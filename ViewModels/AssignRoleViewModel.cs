using System.ComponentModel.DataAnnotations;

namespace ETicaret.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; } = default!;

        public string Email { get; set; } = default!;

        [Display(Name = "Seçilen Rol")]
        public string SelectedRole { get; set; } = default!;

        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
