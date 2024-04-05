using EduPortal.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPortal.Models.Configurations
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IUserEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
           
        }
    }
}
