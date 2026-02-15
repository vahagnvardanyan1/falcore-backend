using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class VTSContext(DbContextOptions<VTSContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehiclePart> VehicleParts { get; set; }
    public DbSet<VehicleInsurance> VehicleInsurances { get; set; }
    public DbSet<VehicleTechnicalInspection> VehicleTechnicalInspections { get; set; }
    public DbSet<GpsPosition> GpsPositions { get; set; }
    public DbSet<GeoFence> GeoFences { get; set; }
    public DbSet<FuelAlert> FuelAlerts { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedDateUtc))
                    .IsRequired();

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.ModifiedDateUtc))
                    .IsRequired();
            }
        }

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                    .HasMaxLength(200)
                    .IsRequired();

            entity.HasMany(x => x.Vehicles)
                    .WithOne(x => x.Tenant)
                    .HasForeignKey(x => x.TenantId)
                    .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.PlateNumber)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.VIN)
                .HasMaxLength(50);

            entity.Property(x => x.Model)
                .HasMaxLength(100);

            entity.HasIndex(x => new { x.TenantId, x.PlateNumber })
                .IsUnique();

            entity.HasMany(x => x.Positions)
                .WithOne()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Parts)
                .WithOne()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.GeoFences)
                .WithOne()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Insurances)
                .WithOne()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GpsPosition>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Location)
                .HasColumnType("geography (point)")
                .IsRequired();

            entity.Property(x => x.TimestampUtc)
                .IsRequired();

            entity.HasIndex(x => x.VehicleId);
        });

        modelBuilder.Entity<VehiclePart>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.PartNumber)
                .HasMaxLength(100);

            entity.HasIndex(x => new { x.VehicleId, x.Name });
        });

        modelBuilder.Entity<VehicleInsurance>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Provider)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.ExpiryDate)
                .HasColumnType("date")
                .HasConversion(
                    d => d,
                    d => d == DateOnly.MinValue ? null : d
                );
        });

        modelBuilder.Entity<VehicleTechnicalInspection>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ExpiryDate)
                .HasColumnType("date")
                .IsRequired();
        });

        modelBuilder.Entity<GeoFence>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Center)
                .HasColumnType("geography (point)")
                .IsRequired();

            entity.Property(x => x.RadiusMeters)
                .IsRequired();

            entity.HasOne(x => x.Vehicle)
                .WithMany(v => v.GeoFences)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FuelAlert>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.ThresholdValue)
                .IsRequired();

            entity.Property(x => x.AlertType)
                .IsRequired();

            entity.HasOne(x => x.Vehicle)
                .WithMany(v => v.FuelAlerts)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.VehicleId, x.Name });
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Message)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(x => x.TimestampUtc)
                .IsRequired();

            entity.Property(x => x.VehicleId)
                .IsRequired();

            entity.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Vehicle)
                .WithMany()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.IsRead).HasDefaultValue(false);

            entity.HasIndex(x => x.TenantId);
            entity.HasIndex(x => x.VehicleId);
            entity.HasIndex(x => x.TimestampUtc);
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDateUtc = utcNow;
                entry.Entity.ModifiedDateUtc = utcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDateUtc = utcNow;
                entry.Property(x => x.CreatedDateUtc).IsModified = false;
            }
        }
    }
}
