using Microsoft.EntityFrameworkCore;
using mpk_lodz_parser_net.infrastructure.Interfaces;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.infrastructure.Repositories;

public class StopRepository(MpkContext context) : IStopRepository
{
    public async Task<Stop> GetStopById(int id) 
        => (await context.Stops.SingleOrDefaultAsync(s => s.Id == id))!;

    public async Task<Stop> GetStopByNumber(int number) 
        => (await context.Stops.SingleOrDefaultAsync(s => s.Number == number))!;

    public IAsyncEnumerable<Stop> GetAllStops() 
        => context.Stops.AsAsyncEnumerable();

    public async Task AddStop(Stop stop) 
        => await context.Stops.AddAsync(stop);

    public async Task AddStopRange(IEnumerable<Stop> stops)
        => await context.Stops.AddRangeAsync(stops);

    public async Task SaveChangesAsync() 
        => await context.SaveChangesAsync();
}