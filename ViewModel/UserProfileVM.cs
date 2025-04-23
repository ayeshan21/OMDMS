using System.ComponentModel.DataAnnotations;

namespace Online_Medicine_Donation.ViewModel
{
    public class UserProfileVM
    {
        [Key]
        public int UserProfileId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public string? FullName { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        public string? NgoCode { get; set; } 

        public IFormFile? ProfilePictureFile { get; set; } 

        public string? ProfilePictureUrl { get; set; }   

        [Required]
        public string? Role { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
    }
}

