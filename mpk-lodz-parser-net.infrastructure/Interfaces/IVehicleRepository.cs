using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.infrastructure.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle> GetVehicleById(int id);
    Task<Vehicle> GetVehicleByNumber(string number);
    IAsyncEnumerable<Vehicle> GetAllVehicles();
    IAsyncEnumerable<Vehicle> GetVehiclesByType(VehicleType type);
    Task AddVehicle(Vehicle vehicle);
    Task AddVehicleRange(IEnumerable<Vehicle> vehicles);
    Task SaveChangesAsync();
}