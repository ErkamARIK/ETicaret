using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        public string Name { get; set; } = default!;

        public List<Product> Products { get; set; } = new();
    }
}
