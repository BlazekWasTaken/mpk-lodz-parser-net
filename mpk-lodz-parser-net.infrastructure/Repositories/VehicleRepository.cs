using Microsoft.EntityFrameworkCore;
using mpk_lodz_parser_net.infrastructure.Interfaces;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.infrastructure.Repositories;

public class VehicleRepository(MpkContext context) : IVehicleRepository
{
    public async Task<Vehicle> GetVehicleById(int id) 
        => (await context.Vehicles.FindAsync(id))!;

    public async Task<Vehicle> GetVehicleByNumber(string number) 
        => (await context.Vehicles.SingleOrDefaultAsync(v => v.Number == number))!;

    public IAsyncEnumerable<Vehicle> GetAllVehicles() 
        => context.Vehicles.AsAsyncEnumerable();

    public IAsyncEnumerable<Vehicle> GetVehiclesByType(VehicleType type) 
        => context.Vehicles.Where(v => v.Type == type).AsAsyncEnumerable();

    public async Task AddVehicle(Vehicle vehicle) 
        => await context.Vehicles.AddAsync(vehicle);

    public async Task SaveChangesAsync()  
        => await context.SaveChangesAsync();
}