using System.Net;

namespace CaseStudy.Api.Helpers;

public static class IPAddressHelper
{
    public static string GenerateOpenApiUri(string uri)
    {
        var ub = new UriBuilder(uri);

        var idpListenIp = IPAddress.Parse(ub.Host);
        var isLocalhost = IPAddress.IsLoopback(idpListenIp) || idpListenIp.ToString() == IPAddress.Any.ToString();

        if (isLocalhost)
        {
            ub.Host = "localhost";
            return ub.Uri.ToString();
        }
        return ub.Uri.ToString();

    }
}