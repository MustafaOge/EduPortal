using EduPortal.Domain.Entities;
using EduPortal.Models.Entities;

namespace EduPortal.Models.Entities
{
    public class AppUserProfile : BaseUserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Relational Properties
        public virtual AppUser AppUser { get; set; }

    }
}
