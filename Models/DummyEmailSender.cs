using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Online_Medicine_Donation.Models
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Log or do nothing
            return Task.CompletedTask;
        }
    }
}
