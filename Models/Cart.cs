using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicaret.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string UserId { get; set; } = default!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;

        public List<CartItem> Items { get; set; } = new();
    }
}
