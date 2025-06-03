using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; } = default!;

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = default!;

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
        
        public List<Comment> Comments { get; set; } = new();


    }
}
