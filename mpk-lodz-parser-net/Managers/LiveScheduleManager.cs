using System.Xml;
using mpk_lodz_parser_net.Helpers;
using mpk_lodz_parser_net.Model;

namespace mpk_lodz_parser_net.Managers;

public abstract class LiveScheduleManager
{
    public static async Task<Schedule> GetNextDepartures(int stopNum, RequestHelper requestHelper)
    {
        var xml = await requestHelper.GetRequest("Home/GetTimetableReal");
        return ParseXml(xml);
    }
    
    private static Schedule ParseXml(string xml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var schedule = new Schedule();
        
        var scheduleNode = doc.DocumentElement?.SelectSingleNode("/Schedules");
        if (scheduleNode == null)
            throw new Exception("No response xml returned");
        var parseSuccess = TimeOnly.TryParse(scheduleNode.Attributes?.GetNamedItem("time")?.InnerText, out var timeNow);
        if (!parseSuccess)
            throw new Exception("Time attribute incorrect");
        schedule.Time = timeNow;
        
        var stopNode = doc.DocumentElement?.SelectSingleNode("/Schedules/Stop");
        if (stopNode == null)
            throw new Exception("No response xml returned");
        // schedule.StopNum = TODO: get stop number
        schedule.StopName = stopNode.Attributes?.GetNamedItem("name")?.InnerText;
        schedule.Comment = stopNode.Attributes?.GetNamedItem("ds")?.InnerText;
        
        var arrivalNodes = doc.SelectNodes("/Schedules/Stop/Day/R");
        if (arrivalNodes == null)
            throw new Exception("No response xml returned");
        
        foreach (var arrivalNode in arrivalNodes.OfType<XmlNode>())
        {
            var arrival = new Departure
            {
                Number = arrivalNode.Attributes?.GetNamedItem("nr")?.InnerText,
                Direction = arrivalNode.Attributes?.GetNamedItem("dir")?.InnerText
            };

            var childNode = arrivalNode.FirstChild;
            if (childNode == null)
                throw new Exception("No response xml returned");
            
            var th = childNode.Attributes?.GetNamedItem("th")?.InnerText; // hour of arrival or empty if (I think) closer than 20 minutes
            var tm = childNode.Attributes?.GetNamedItem("tm")?.InnerText; // minute of arrival or minutes till arrival with "min" if th empty
            var t = childNode.Attributes?.GetNamedItem("t")?.InnerText; // hh:mm or same as tm if th empty
            var uw = childNode.Attributes?.GetNamedItem("uw")?.InnerText; // yyyy-MM-dd if different from today
            arrival.WhenText = t;
            
            var date = uw == string.Empty ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.Parse(uw);
            var time = th == string.Empty ? 
                TimeOnly.FromDateTime(DateTime.Now).AddMinutes(int.Parse(tm.Split(' ')[0])) : 
                TimeOnly.Parse($"{th}:{tm}");
            arrival.When = new DateTime(date, time);
            
            schedule.Departures.Add(arrival);
        }
        return schedule;
    }
}