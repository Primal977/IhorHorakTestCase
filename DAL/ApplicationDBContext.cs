using DAL.Entities;
using DAL.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ApplicationDBContext : DbContext
{
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<BookVolume> BookVolumes { get; set; }

    public ApplicationDBContext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new BookVolumeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
