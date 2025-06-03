using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicaret.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yorum boş olamaz.")]
        public string Text { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = default!;

        public string UserId { get; set; } = default!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;
    }
}
