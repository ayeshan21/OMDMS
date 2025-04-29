using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Online_Medicine_Donation.DataModel.Online_Medicine_Donation.Models;
using Online_Medicine_Donation.DataModel;

namespace Online_Medicine_Donation.Data
{
    public class OnlineMedicineContext : IdentityDbContext
    {
        public OnlineMedicineContext(DbContextOptions<OnlineMedicineContext> options)
            : base(options)
        {

        }

        public DbSet<UserProfile>UserProfiles { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
    }

}
