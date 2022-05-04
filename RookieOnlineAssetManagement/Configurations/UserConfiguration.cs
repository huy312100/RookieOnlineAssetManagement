using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookieOnlineAssetManagement.Entities;

namespace RookieOnlineAssetManagement.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Serial).UseIdentityColumn();
            builder.Property(x => x.Serial).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property(x => x.UserCode).HasComputedColumnSql("'SD'+ RIGHT('0000'+CAST(Serial AS VARCHAR(4)),4)");
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);
            builder.HasOne(x => x.Location).WithMany(x => x.Users).HasForeignKey(x => x.LocationId);
        }
    }
}