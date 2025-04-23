using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;

namespace Online_Medicine_Donation.Data
{
    public class OnlineMedicineContext : IdentityDbContext
    {
        public OnlineMedicineContext(DbContextOptions<OnlineMedicineContext> options)
            : base(options)
        {

        }

        public DbSet<UserProfile>UserProfiles { get; set; }
    }

}
