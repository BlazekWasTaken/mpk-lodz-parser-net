using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.infrastructure.Interfaces;

public interface IStopRepository
{
    Task<Stop> GetStopById(int id);
    Task<Stop> GetStopByNumber(int number);
    IQueryable<Stop> GetAllStops();
    Task AddStop(Stop stop);
    Task AddStopRange(IEnumerable<Stop> stops);
    Task SaveChangesAsync();
}