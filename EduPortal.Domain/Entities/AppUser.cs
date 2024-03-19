using Microsoft.AspNetCore.Identity;

namespace EduPortal.API.Models
{
    public class AppUser : IdentityUser<Guid>
    {

        public int City { get; set; }

        public DateTime? BirthDate { get; set; }

    }
}