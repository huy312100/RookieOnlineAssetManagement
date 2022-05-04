using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookieOnlineAssetManagement.Entities;

namespace RookieOnlineAssetManagement.Configurations
{
    public class ReturnAssetConfiguration : IEntityTypeConfiguration<ReturnAsset>
    {
        public void Configure(EntityTypeBuilder<ReturnAsset> builder)
        {
            builder.ToTable("ReturnAssets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(u => u.User).WithMany(r => r.ReturnAssets).HasForeignKey(r => r.UserId);
            builder.HasOne(a => a.Assignment).WithMany(r => r.ReturnAssets).HasForeignKey(r => r.AssignmentId);
        }
    }
}