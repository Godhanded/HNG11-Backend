using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserOrgs.Data;

namespace UserOrgs.Configurations
{
    public class UserOrganisationConfig : IEntityTypeConfiguration<UserOrganisation>
    {
        public void Configure(EntityTypeBuilder<UserOrganisation> builder)
        {
            builder.HasKey(uo => new { uo.userId, uo.organisationId });

            builder.HasOne(uo => uo.user)
                .WithMany(u => u.userOrganisations)
                .HasForeignKey(uo => uo.userId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(uo => uo.organisation)
                .WithMany(o => o.userOrganisations)
                .HasForeignKey(uo => uo.organisationId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
