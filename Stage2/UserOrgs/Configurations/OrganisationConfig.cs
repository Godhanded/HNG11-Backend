using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserOrgs.Data;

namespace UserOrgs.Configurations
{
    public class OrganisationConfig : IEntityTypeConfiguration<Organisation>
    {
        public void Configure(EntityTypeBuilder<Organisation> builder)
        {
            builder.HasKey(o=>o.orgId);
            builder.Property(o=>o.name)
                .IsRequired();

        }
    }
}
