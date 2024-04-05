using EduPortal.Domain.Abstractions;
using EduPortal.Models.Entities;
using EduPortal.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace EduPortal.Models.Entities
{
    public class AppRole : IdentityRole<int>, IUserEntity
    {
        public AppRole()
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

    }
}
