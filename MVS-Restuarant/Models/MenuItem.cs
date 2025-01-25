using System.ComponentModel.DataAnnotations;

namespace MVS_Restuarant.Models
{
    public class MenuItem
    {
        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string ItemName { get; set; } // Name of the dish or drink

        [Required(ErrorMessage = "*Required")]
        public string ItemDescription { get; set; } // Description of the item

        [Required(ErrorMessage = "*Required")]
        public decimal ItemPrice { get; set; } // Price of the item

        [Required(ErrorMessage = "*Required")]
        public string Category { get; set; } // Category (e.g., Appetizer, Main, Dessert, Drink)

        [Required(ErrorMessage = "*Required")]
        public byte[] ImageData { get; set; } // Binary data for the image


        public string ContentType { get; set; } // e.g., "image/png", "image/jpeg"

        public bool IsAvailable { get; set; } = true;// Indicates if the item is available
    }
}
