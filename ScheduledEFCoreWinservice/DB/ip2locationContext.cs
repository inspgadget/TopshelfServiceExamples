using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ScheduledEFCoreWinservice.DB
{
public partial class ip2locationContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public ip2locationContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ip2locationContext(DbContextOptions<ip2locationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ip2locationDb11> Ip2locationDb11 { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CTX"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ip2locationDb11>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ip2location_db11");

            entity.HasIndex(e => e.IpFrom)
                .HasName("ip_from");

            entity.HasIndex(e => e.IpTo)
                .HasName("ip_to");

            entity.Property(e => e.CityName)
                .IsRequired()
                .HasColumnName("city_name")
                .HasMaxLength(128);

            entity.Property(e => e.CountryCode)
                .IsRequired()
                .HasColumnName("country_code")
                .HasMaxLength(2);

            entity.Property(e => e.CountryName)
                .IsRequired()
                .HasColumnName("country_name")
                .HasMaxLength(64);

            entity.Property(e => e.IpFrom).HasColumnName("ip_from");

            entity.Property(e => e.IpTo).HasColumnName("ip_to");

            entity.Property(e => e.Latitude).HasColumnName("latitude");

            entity.Property(e => e.Longitude).HasColumnName("longitude");

            entity.Property(e => e.RegionName)
                .IsRequired()
                .HasColumnName("region_name")
                .HasMaxLength(128);

            entity.Property(e => e.TimeZone)
                .IsRequired()
                .HasColumnName("time_zone")
                .HasMaxLength(8);

            entity.Property(e => e.ZipCode)
                .IsRequired()
                .HasColumnName("zip_code")
                .HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private static readonly object _lock = new object();

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override void Dispose()
    {
        Console.WriteLine("CONTEXT DISPOSED");
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        Console.WriteLine("CONTEXT DISPOSED");
        return base.DisposeAsync();
    }
}
}
