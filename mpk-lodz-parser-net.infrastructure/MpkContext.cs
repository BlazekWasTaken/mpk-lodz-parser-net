using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.infrastructure;

public class MpkContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        options.UseSqlite(connection).EnableSensitiveDataLogging();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stop>()
            .HasMany(e => e.Vehicles)
            .WithMany(x => x.Stops);
    }

    
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Stop> Stops { get; set; }
}