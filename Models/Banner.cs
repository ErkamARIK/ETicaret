using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Banner
    {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
