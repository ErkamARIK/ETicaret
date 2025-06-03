using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ETicaret.Models;

namespace ETicaret.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        public string UserId { get; set; } = default!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;

        public string CardHolder { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public string Expiration { get; set; } = "";
        public string CVV { get; set; } = "";
    }
}
