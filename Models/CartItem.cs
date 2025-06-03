using System.ComponentModel.DataAnnotations.Schema;

namespace ETicaret.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = default!;

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = default!;

        public int Quantity { get; set; } = 1;
    }
}
