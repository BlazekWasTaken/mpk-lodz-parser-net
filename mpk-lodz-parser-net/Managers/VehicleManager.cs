using System.Text.Json;
using System.Text.Json.Nodes;
using mpk_lodz_parser_net.Helpers;
using mpk_lodz_parser_net.infrastructure;
using mpk_lodz_parser_net.infrastructure.Interfaces;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.Managers;

public abstract class VehicleManager
{
    public static async Task CreateVehicles(IVehicleRepository vehicleRepository, RequestHelper requestHelper)
    {
        var json =  await requestHelper.PostRequest("Home/GetRouteList"); 
        var vehicles = ParseJson(json);
        await vehicleRepository.AddVehicleRange(vehicles);
        await vehicleRepository.SaveChangesAsync();
    }

    private static List<Vehicle> ParseJson(string json)
    {
        var root = JsonNode.Parse(json);
        if (root!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        List<Vehicle> vehicles = [];

        if (root.AsArray().Count != 2) 
            throw new Exception("List must have 2 elements");
        if (root[0][1].GetValueKind() != JsonValueKind.Array || root[1][1].GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        
        var busList = root[0][1].AsArray();
        for (int i = 0; i < busList.Count; i += 2)
        {
            var bus = new Vehicle();
            if (busList[i].GetValueKind() != JsonValueKind.Number)
                throw new Exception("Json is not a number");
            bus.Id = busList[i].GetValue<int>();
            if (busList[i + 1].GetValueKind() != JsonValueKind.String)
                throw new Exception("Json is not a string");
            bus.Number = busList[i + 1].GetValue<string>();
            bus.Type = VehicleType.Bus;
            vehicles.Add(bus);
        }
        var tramList = root[1][1].AsArray();
        for (int i = 0; i < tramList.Count; i += 2)
        {
            var tram = new Vehicle();
            if (tramList[i].GetValueKind() != JsonValueKind.Number)
                throw new Exception("Json is not a number");
            tram.Id = tramList[i].GetValue<int>();
            if (tramList[i + 1].GetValueKind() != JsonValueKind.String)
                throw new Exception("Json is not a string");
            tram.Number = tramList[i + 1].GetValue<string>();
            tram.Type = VehicleType.Tram;
            vehicles.Add(tram);
        }
        return vehicles;
    }
}