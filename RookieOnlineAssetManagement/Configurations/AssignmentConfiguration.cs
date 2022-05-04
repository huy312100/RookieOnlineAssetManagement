using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookieOnlineAssetManagement.Entities;

namespace RookieOnlineAssetManagement.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("Assignments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(a => a.User).WithMany(u => u.Assignments).HasForeignKey(u => u.UserId);
            builder.HasOne(a => a.Asset).WithMany(u => u.Assignments).HasForeignKey(u => u.AssetId);
        }
    }
}