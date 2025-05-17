using System.Web;
using Flurl.Http;

namespace mpk_lodz_parser_net;

public class RequestHelper(string host = "rozklady.lodz.pl", string path = "Home/GetTimetableReal", Dictionary<string, string>? query = null)
{
    private string QueryString()
    {
        if (query == null)
            return "";
        var queries = query.Keys.Select(key => $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(query[key])}");
        return string.Join("&", queries);
    }
    public Uri GetUri()
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = Uri.UriSchemeHttp,
            Host = host,
            Path = path,
            Query = QueryString()
        };
        var uri = uriBuilder.Uri;
        return uri;
    }
    public async Task<string> GetResponseXml()
    {
        var responseXml = await GetUri()
            .GetStringAsync();
        if (string.IsNullOrEmpty(responseXml))
            throw new Exception("No response xml returned");
        
        return responseXml;
    }
}