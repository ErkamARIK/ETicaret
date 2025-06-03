using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public int AddressId { get; set; }
        public Address Address { get; set; } = default!;

        public int CardId { get; set; }
        public CreditCard Card { get; set; } = default!;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
