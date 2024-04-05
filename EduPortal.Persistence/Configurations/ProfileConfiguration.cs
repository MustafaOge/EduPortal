using EduPortal.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPortal.Models.Configurations
{
    public class ProfileConfiguration:BaseConfiguration<AppUserProfile>
    {
        public override void Configure(EntityTypeBuilder<AppUserProfile> builder)
        {
            base.Configure(builder);

        }
    }
}
