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

        }
    }
}
