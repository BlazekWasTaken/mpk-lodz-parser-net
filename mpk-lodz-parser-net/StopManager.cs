using System.Text.Json;
using System.Text.Json.Nodes;
using mpk_lodz_parser_net.Model;

namespace mpk_lodz_parser_net;

public class StopManager
{
    public List<Stop> Stops { get; }

    private StopManager(List<Stop> stops)
    {
        Stops = stops;
    }

    public static async Task<StopManager> Create()
    {
        var helper = RequestHelper.Create("rozklady.lodz.pl");
        var json =  await helper.PostRequest("Home/GetMapBusStopList");
        var list = ParseJson(json);
        return new StopManager(list);
    }

    private static List<Stop> ParseJson(string json)
    {
        var root = JsonNode.Parse(json);
        if (root!.GetValueKind() != JsonValueKind.Array) throw new Exception("Json is not an array");
        List<Stop> stops = [];
        foreach (var node in root.AsArray())
        {
            var stop = new Stop();
            var list = node.AsArray();
            
            if (list.GetValueKind() != JsonValueKind.Array) throw new Exception("Json is not an array");
            if (list.AsArray().Count != 8) throw new Exception("Stop list must have 8 elements");
            
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
                list[7][2].GetValueKind() != JsonValueKind.Array) throw new Exception("Json is not an array");

            if (list[7][0].AsArray().Count == 2)
            {
                if (list[7][0][1].GetValueKind() != JsonValueKind.Array) throw new Exception("Json is not an array");
                var buses =  list[7][0][1].AsArray();
                for (int i = 0; i < buses.Count; i += 2)
                {
                    var vehicle = new Vehicle();
                    if (buses[i]!.GetValueKind() != JsonValueKind.Number) throw new Exception("Json is not a number");
                    vehicle.Id = buses[i]!.GetValue<int>();
                    if (buses[i + 1]!.GetValueKind() != JsonValueKind.String) throw new Exception("Json is not a string");
                    vehicle.Number = buses[i + 1]!.GetValue<string>();
                    vehicle.Type = VehicleType.Bus;
                    stop.Vehicles.Add(vehicle);
                }
            }
            if (list[7][1].AsArray().Count == 2)
            {
                if (list[7][1][1].GetValueKind() != JsonValueKind.Array) throw new Exception("Json is not an array");
                var trams =  list[7][1][1].AsArray();
                for (int i = 0; i < trams.Count; i += 2)
                {
                    var vehicle = new Vehicle();
                    if (trams[i]!.GetValueKind() != JsonValueKind.Number) throw new Exception("Json is not a number");
                    vehicle.Id = trams[i]!.GetValue<int>();
                    if (trams[i + 1]!.GetValueKind() != JsonValueKind.String) throw new Exception("Json is not a string");
                    vehicle.Number = trams[i + 1]!.GetValue<string>();
                    vehicle.Type = VehicleType.Tram;
                    stop.Vehicles.Add(vehicle);
                }
            }
            if (list[7][2].AsArray().Count == 2)
            {
                if (list[7][2][1].GetValueKind() != JsonValueKind.Array) throw new Exception("Json is not an array");
                var idk =  list[7][2][1].AsArray(); // TODO: find out what is in the third array and parse
            }
            
            stops.Add(stop);
        }
        return stops;
    }
}