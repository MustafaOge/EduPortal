using EduPortal.Models.Enums;
using EduPortal.Domain.Abstractions;

namespace EduPortal.Models.Entities
{
    public abstract class BaseUserEntity : IUserEntity
    {
        public BaseUserEntity()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;
        }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }
    }
}
