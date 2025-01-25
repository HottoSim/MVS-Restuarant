using MVS_Restuarant.Data;
using System.ComponentModel.DataAnnotations;

namespace MVS_Restuarant.Models.Account
{
    public class Admin
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "*Required")]

        [RegularExpression(@"^\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])\d{4}[01]8\d$",
    ErrorMessage = "The ID Number is not in a valid South African format.")]
        public string AdminIDNumber { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
