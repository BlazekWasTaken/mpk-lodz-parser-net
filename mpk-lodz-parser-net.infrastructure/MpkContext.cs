using Microsoft.EntityFrameworkCore;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.infrastructure;

public class MpkContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Filename=:memory:");
    }
    
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Stop> Stops { get; set; }
}