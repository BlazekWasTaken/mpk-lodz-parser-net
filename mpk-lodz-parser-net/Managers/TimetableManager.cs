using System.Text.Json;
using System.Text.Json.Nodes;
using mpk_lodz_parser_net.Helpers;
using mpk_lodz_parser_net.infrastructure.Interfaces;
using mpk_lodz_parser_net.infrastructure.Model;
using mpk_lodz_parser_net.Model;

namespace mpk_lodz_parser_net.Managers;

public class TimetableManager
{
    // TODO: get timetables from bus stops: http://rozklady.lodz.pl/Home/GetBusStopTimeTable?busStopId=2781&routeId=377233
    
    public static async Task<Timetable> GetTimetable(
        int stopId, int vehicleId, 
        RequestHelper requestHelper, 
        IStopRepository stopRepository, IVehicleRepository vehicleRepository)
    {
        var xml = await requestHelper.GetRequest("Home/GetBusStopTimeTable", new Dictionary<string, string>
        {
            { "busStopId", stopId.ToString() },
            { "routeId", vehicleId.ToString() }
        });
        var stop = await stopRepository.GetStopByNumber(stopId);
        var vehicle = await vehicleRepository.GetVehicleById(vehicleId);
        return ParseJson(xml, stop, vehicle);
    }

    private static Timetable ParseJson(string json, Stop stop, Vehicle vehicle)
    {
        var root = JsonNode.Parse(json);
        if (root!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        if (root.AsArray().Count != 4)
            throw new Exception("Root must have 4 elements");
        
        var timetable = new Timetable
        {
            Stop = stop,
            Vehicle = vehicle
        };
        
        if (root[2]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        foreach (var node in root[2]!.AsArray())
        {
            if (node!.GetValueKind() != JsonValueKind.Array) 
                throw new Exception("Json is not an array");
            if (node.AsArray().Count != 9)
                throw new Exception("List must have 9 elements");
            
            if (node[1]!.GetValueKind() != JsonValueKind.Number)
                throw new Exception("Json is not a number");
            if (node[3]!.GetValueKind() != JsonValueKind.String ||
                node[8]!.GetValueKind() != JsonValueKind.String)
                throw new Exception("Json is not a string");
            var subroute = new TimetableSubroute
            {
                SubrouteId = node[1]!.GetValue<int>(),
                Name = node[3]!.GetValue<string>(),
                Overview = node[8]!.GetValue<string>()
            };
            timetable.Subroutes.Add(subroute);
        }
        
        if (root[3]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        if (root[3]!.AsArray().Count != 3)
            throw new Exception("List must have 3 elements");
        
        if (root[3]![0]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        if (root[3]![0]!.AsArray().Count != 6)
            throw new Exception("List must have 6 elements");
        if (root[3]![0]![4]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        foreach (var node in root[3]![0]![4]!.AsArray())
        {
            if (node![0]!.GetValueKind() != JsonValueKind.Number)
                throw new Exception("Json is not a number");
            var subrouteId = node[0]!.GetValue<int>();
            
            if (node[2]!.GetValueKind() != JsonValueKind.String)
                throw new Exception("Json is not a string");
            var timeString = node[2]!.GetValue<string>();
            var timeNumber = double.Parse(timeString);
            var hour = Math.Floor(timeNumber / 60);
            var minute = timeNumber % 60;
            var time = new TimeOnly(hour: (int)hour, minute: (int)minute);
            timetable.Subroutes.First(x => x.SubrouteId == subrouteId).TimesOnWeekdays.Add(time);
        }
        
        if (root[3]![1]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        if (root[3]![1]!.AsArray().Count != 6)
            throw new Exception("List must have 6 elements");
        if (root[3]![1]![4]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        foreach (var node in root[3]![1]![4]!.AsArray())
        {
            if (node![0]!.GetValueKind() != JsonValueKind.Number)
                throw new Exception("Json is not a number");
            var subrouteId = node[0]!.GetValue<int>();
            
            if (node[2]!.GetValueKind() != JsonValueKind.String)
                throw new Exception("Json is not a string");
            var timeString = node[2]!.GetValue<string>();
            var timeNumber = double.Parse(timeString);
            var hour = Math.Floor(timeNumber / 60);
            var minute = timeNumber % 60;
            var time = new TimeOnly(hour: (int)hour, minute: (int)minute);
            timetable.Subroutes.First(x => x.SubrouteId == subrouteId).TimesOnSaturdays.Add(time);
        }
        
        if (root[3]![2]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        if (root[3]![2]!.AsArray().Count != 6)
            throw new Exception("List must have 6 elements");
        if (root[3]![2]![4]!.GetValueKind() != JsonValueKind.Array) 
            throw new Exception("Json is not an array");
        foreach (var node in root[3]![2]![4]!.AsArray())
        {
            if (node![0]!.GetValueKind() != JsonValueKind.Number)
                throw new Exception("Json is not a number");
            var subrouteId = node[0]!.GetValue<int>();
            
            if (node[2]!.GetValueKind() != JsonValueKind.String)
                throw new Exception("Json is not a string");
            var timeString = node[2]!.GetValue<string>();
            var timeNumber = double.Parse(timeString);
            var hour = Math.Floor(timeNumber / 60);
            var minute = timeNumber % 60;
            var time = new TimeOnly(hour: (int)hour, minute: (int)minute);
            timetable.Subroutes.First(x => x.SubrouteId == subrouteId).TimesOnSundays.Add(time);
        }

        return timetable;
    }
}