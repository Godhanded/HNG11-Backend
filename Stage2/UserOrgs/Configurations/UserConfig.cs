using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserOrgs.Data;

namespace UserOrgs.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.userId);
            builder.HasIndex(u => u.email)
                .IsUnique();
            builder.Property(u => u.email)
                .IsRequired();
            builder.Property(u => u.password)
                .IsRequired();
            builder.Property(u=>u.passwordSalt)
                .IsRequired();
            builder.Property(u=>u.firstName)
                .IsRequired();
            builder.Property(u => u.lastName)
                .IsRequired();
                
        }
    }
}
