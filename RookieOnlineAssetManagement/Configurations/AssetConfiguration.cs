using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookieOnlineAssetManagement.Entities;

namespace RookieOnlineAssetManagement.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.Category).WithMany(x => x.Assets).HasForeignKey(x => x.CategoryId);
        }
    }
}