namespace MVS_Restuarant.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; } // Reference to the menu item
        public virtual MenuItem MenuItem { get; set; } // Navigation property
        public int Quantity { get; set; } // Quantity of the item
        public decimal TotalPrice => Quantity * MenuItem.ItemPrice; // Computed property for total price
    }
}

