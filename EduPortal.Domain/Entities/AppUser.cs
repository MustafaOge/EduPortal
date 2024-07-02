using EduPortal.Domain.Abstractions;
using EduPortal.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace EduPortal.Models.Entities
{
    public class AppUser : IdentityUser<int>, IUserEntity
    {
        public AppUser()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;
        }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        //Relational Properties
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual AppUserProfile Profile { get; set; }
    }
}
