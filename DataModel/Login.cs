using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Online_Medicine_Donation.DataModel
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please fill out this field")]
        [EmailAddress]
        [StringLength(20,ErrorMessage = "E-mail must be less than or equal to 20 characters")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please fill out this field")]
        [StringLength(10, ErrorMessage = "Password must be less than or equal to 10 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please select the role")]
        public string? Role { get; set; }

    }
}
