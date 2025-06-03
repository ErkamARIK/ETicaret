using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ETicaret.Models;
namespace ETicaret.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public string Title { get; set; } = "";
        public string FullAddress { get; set; } = "";
        public string City { get; set; } = "";
        public string PostalCode { get; set; } = "";
    }
}
