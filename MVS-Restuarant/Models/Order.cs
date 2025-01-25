using MVS_Restuarant.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVS_Restuarant.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string OrderNumber { get; set; } // Unique order identifier
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        [ForeignKey("ApplicationUser")]
        public string CustomerId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string OrderType { get; set; } // Delivery, Collection, or Sit-In
        public string Address { get; set; } // Delivery address (for delivery orders)

        public DateTime OrderDate { get; set; }

        public List<OrderItem> OrderItems { get; set; } // List of ordered items
        public decimal TotalAmount { get; set; } // Total amount for the order
        public string Status { get; set; } // Pending, In-Progress, Completed
    }
}
