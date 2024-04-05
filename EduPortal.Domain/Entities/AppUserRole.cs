using EduPortal.Models.Enums;
using EduPortal.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace EduPortal.Models.Entities
{
    public class AppUserRole : IdentityUserRole<int>, IUserEntity
    {
        public AppUserRole()
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
        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }

    }
}
