using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionsService.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public string Type { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        public string? Detail { get; set; }
    }
}
