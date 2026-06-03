using Microsoft.EntityFrameworkCore;
using WebApplication2.Entities;

namespace WebApplication2.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<WashingProgram> Programs { get; set; }
    public DbSet<WashingMachine> WashingMachines { get; set; }
    public DbSet<AvailableProgram> AvailablePrograms { get; set; }
    public DbSet<PurchaseHistory> PurchaseHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(opt =>
        {
            opt.ToTable("Customer");
            opt.HasKey(e => e.CustomerId);
            opt.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            opt.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            opt.Property(e => e.PhoneNumber).HasMaxLength(100);
        });

        modelBuilder.Entity<WashingProgram>(opt =>
        {
            opt.ToTable("Program");
            opt.HasKey(e => e.ProgramId);
            opt.Property(e => e.Name).HasMaxLength(50).IsRequired();
            opt.Property(e => e.DurationMinutes).IsRequired();
            opt.Property(e => e.TemperatureCelsius).IsRequired();
        });

        modelBuilder.Entity<WashingMachine>(opt =>
        {
            opt.ToTable("Washing_Machine");
            opt.HasKey(e => e.WashingMachineId);
            opt.Property(e => e.MaxWeight).HasColumnType("decimal(10,2)").IsRequired();
            opt.Property(e => e.SerialNumber).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<AvailableProgram>(opt =>
        {
            opt.ToTable("Available_Program");
            opt.HasKey(e => e.AvailableProgramId);
            opt.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();

            opt.HasOne(e => e.WashingMachine)
                .WithMany(w => w.AvailablePrograms)
                .HasForeignKey(e => e.WashingMachineId);

            opt.HasOne(e => e.Program)
                .WithMany(p => p.AvailablePrograms)
                .HasForeignKey(e => e.ProgramId);
        });

        modelBuilder.Entity<PurchaseHistory>(opt =>
        {
            opt.ToTable("Purchase_History");
            opt.HasKey(e => new { e.AvailableProgramId, e.CustomerId });
            opt.Property(e => e.PurchaseDate).IsRequired();
            opt.Property(e => e.Rating);

            opt.HasOne(e => e.AvailableProgram)
                .WithMany(ap => ap.PurchaseHistories)
                .HasForeignKey(e => e.AvailableProgramId);

            opt.HasOne(e => e.Customer)
                .WithMany(c => c.PurchaseHistories)
                .HasForeignKey(e => e.CustomerId);
        });
    }
}
