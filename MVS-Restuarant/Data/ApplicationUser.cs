using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MVS_Restuarant.Data
{
    public class ApplicationUser: IdentityUser
    {
        [Required(ErrorMessage = "*Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string LastName { get; set; }
    
        public string Role { get; set; } // Customer or Admin or Manager
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool isActive { get; set; } = true;
    }
}
