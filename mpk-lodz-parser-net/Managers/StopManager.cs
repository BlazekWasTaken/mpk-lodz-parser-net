using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using mpk_lodz_parser_net.Helpers;
using mpk_lodz_parser_net.infrastructure;
using mpk_lodz_parser_net.infrastructure.Interfaces;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.Managers;

public abstract class StopManager
{
    public static async Task CreateStops(IStopRepository stopRepository, IVehicleRepository vehicleRepository, RequestHelper requestHelper)
    {
        var json =  await requestHelper.PostRequest("Home/GetMapBusStopList");
        var stops = await ParseJson(vehicleRepository, json);
        await stopRepository.AddStopRange(stops);
        await stopRepository.SaveChangesAsync();
    }
    private static async Task<List<Stop>> ParseJson(IVehicleRepository vehicleRepository, string json)
    {
        var root = JsonNode.Parse(json);
        if (root!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        var stops = new List<Stop>();
        foreach (var node in root.AsArray())
        {
            var stop = new Stop();
            if (node!.GetValueKind() != JsonValueKind.Array) 
                throw new Exception("Json is not an array");
            var list = node.AsArray();
            
            if (list.AsArray().Count != 8) 
                throw new Exception("Stop list must have 8 elements");
            if (list[0]!.GetValueKind() != JsonValueKind.Number ||
                list[4]!.GetValueKind() != JsonValueKind.Number ||
                list[5]!.GetValueKind() != JsonValueKind.Number)
                throw new Exception("Json is not a number");
            if (list[1]!.GetValueKind() != JsonValueKind.String || 
                list[2]!.GetValueKind() != JsonValueKind.String) 
                throw new Exception("Json is not a string");
            
            stop.Id = list[0]!.GetValue<int>();
            stop.Name = list[1]!.GetValue<string>();
            stop.Number = int.Parse(list[2]!.GetValue<string>());
            stop.Longitude = list[4]!.GetValue<float>();
            stop.Latitude = list[5]!.GetValue<float>();
    
            if (list[7].GetValueKind() != JsonValueKind.Array ||
                list[7][0].GetValueKind() != JsonValueKind.Array ||
                list[7][1].GetValueKind() != JsonValueKind.Array ||
                list[7][2].GetValueKind() != JsonValueKind.Array) 
                throw new Exception("Json is not an array");
    
            if (list[7][0].AsArray().Count == 2)
            {
                if (list[7][0][1].GetValueKind() != JsonValueKind.Array) 
                    throw new Exception("Json is not an array");
                var buses =  list[7][0][1].AsArray();
                for (int i = 0; i < buses.Count; i += 2)
                {
                    if (buses[i]!.GetValueKind() != JsonValueKind.Number) 
                        throw new Exception("Json is not a number");
                    var id = buses[i]!.GetValue<int>();
                    var vehicle = await vehicleRepository.GetVehicleById(id);
                    stop.Vehicles.Add(vehicle);
                }
            }
            if (list[7][1].AsArray().Count == 2)
            {
                if (list[7][1][1].GetValueKind() != JsonValueKind.Array) 
                    throw new Exception("Json is not an array");
                var trams =  list[7][1][1].AsArray();
                for (int i = 0; i < trams.Count; i += 2)
                {
                    if (trams[i]!.GetValueKind() != JsonValueKind.Number) 
                        throw new Exception("Json is not a number");
                    var id = trams[i]!.GetValue<int>();
                    var vehicle = await vehicleRepository.GetVehicleById(id);
                    stop.Vehicles.Add(vehicle);
                }
            }
            if (list[7][2].AsArray().Count == 2)
            {
                if (list[7][2][1].GetValueKind() != JsonValueKind.Array) 
                    throw new Exception("Json is not an array");
                var idk =  list[7][2][1].AsArray(); // TODO: find out what is in the third array and parse
            }
            stops.Add(stop);
        }
        return stops;
    }
}