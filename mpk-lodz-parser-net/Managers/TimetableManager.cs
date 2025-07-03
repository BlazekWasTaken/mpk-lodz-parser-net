using mpk_lodz_parser_net.Helpers;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.Managers;

public class TimetableManager
{
    // TODO: get timetables from bus stops: http://rozklady.lodz.pl/Home/GetBusStopTimeTable?busStopId=2781&routeId=377233
    
    public static async Task<Timetable> GetTimetable(int stopId, int vehicleId, RequestHelper requestHelper)
    {
        var xml = await requestHelper.GetRequest("Home/GetBusStopTimeTable", new Dictionary<string, string>
        {
            { "busStopId", stopId.ToString() },
            { "routeId", vehicleId.ToString() }
        });
        return ParseXml(xml);
    }

    private static Timetable ParseXml(string xml)
    {
        return new Timetable();
    }
}