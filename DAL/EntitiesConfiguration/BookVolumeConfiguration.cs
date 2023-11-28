using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntitiesConfiguration
{
    internal class BookVolumeConfiguration : IEntityTypeConfiguration<BookVolume>
    {
        public void Configure(EntityTypeBuilder<BookVolume> builder)
        {
            builder.HasKey(v => new { v.GoogleVolumeID, v.CategoryID });
            builder.Property(v => v.GoogleVolumeID).HasMaxLength(20);

            builder.HasOne(v => v.Category).WithMany(c => c.BookVolumes);
        }
    }
}
