using System.Web;
using Flurl.Http;

namespace mpk_lodz_parser_net;

public class RequestHelper
{
    private readonly string _host;
    private readonly bool _https;
    private readonly int _port;

    private RequestHelper(string host, bool https, int port)
    {
        _host = host;
        _https = https;
        _port = port;
    }
    
    public static RequestHelper Create(string host, bool https = false, int? port = null)
    {
        int portValue;
        if (port is null) portValue = https ? 443 : 80;
        else portValue = port.Value;
        return new RequestHelper(host, https, portValue);
    }
    
    public async Task<string> GetRequest(string path, Dictionary<string, string>? query = null)
    {
        var response = await GetUri(path, QueryString(query))
            .GetStringAsync();
        if (string.IsNullOrEmpty(response))
            throw new Exception("No response xml returned");
        return response;
    }
    public async Task<string> PostRequest(string path)
    {
        var response = 
            await GetUri(path)
                .PostStringAsync("")
                .ReceiveString();
        if (string.IsNullOrEmpty(response))
            throw new Exception("No response xml returned");
        return response;
    }
    
    private static string QueryString(Dictionary<string, string>? query)
    {
        if (query == null)
            return "";
        var queries = query.Keys.Select(key => $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(query[key])}");
        return string.Join("&", queries);
    }
    private Uri GetUri(string path, string query = "")
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = _https ? Uri.UriSchemeHttps : Uri.UriSchemeHttp,
            Host = _host,
            Port = _port,
            Path = path,
            Query = query
        };
        var uri = uriBuilder.Uri;
        return uri;
    }
}