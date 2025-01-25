namespace MVS_Restuarant.Models
{
    public class CartItem
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

