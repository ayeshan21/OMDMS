namespace Online_Medicine_Donation.DataModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    namespace Online_Medicine_Donation.Models
    {
        public class UserProfile
        {
            [Key]
            public int UserProfileId { get; set; }

            public Guid UserId { get; set; }

            [StringLength(50)]
            [Display(Name = "Full Name")]
            public string? FullName { get; set; }

            [EmailAddress]
            [StringLength(100)]
            public string? Email { get; set; }

            [Phone]
            [StringLength(20)]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            [StringLength(200)]
            public string? Address { get; set; }

            [Display(Name = "NGO Code")]
            [StringLength(100)] 
            public string? NgoCode { get; set; }

            [StringLength(500)]
            [Display(Name = "Profile Picture URL")]
            public string? ProfilePictureUrl { get; set; }  

            [Display(Name = "Role")]
            public string? Role { get; set; }

            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
           
        }
    }
}

