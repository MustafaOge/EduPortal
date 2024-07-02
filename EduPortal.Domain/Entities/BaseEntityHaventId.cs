using EduPortal.Domain.Abstractions;

namespace EduPortal.Domain.Entities
{
    public abstract class BaseEntityCustom<TKey> : IEntity
    {
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public int CreatedByUser { get; set; }

        public int? UpdatedByUser { get; set; }
    }
}
