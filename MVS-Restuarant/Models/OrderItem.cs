using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVS_Restuarant.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; } // Reference to the order
        public virtual Order Order { get; set; } // Navigation property

        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; } // Reference to the menu item
        public virtual MenuItem MenuItem { get; set; } // Navigation property

        [Required(ErrorMessage = "*Required")]
        public int Quantity { get; set; } // Quantity of the item

        [Required(ErrorMessage = "*Required")]
        public decimal ItemPrice { get; set; } // Price of the item at the time of order

        public decimal TotalPrice => Quantity * ItemPrice; // Computed property for total price
    }
}
