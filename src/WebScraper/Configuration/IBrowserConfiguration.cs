using System;

namespace WebScraper.Configuration
{
    public interface IBrowserConfiguration
    {
        string ProxyServer { get; }

        string UserAgent { get; }

        IBrowserConfiguration WithProxyServer(string proxyServer);

        IBrowserConfiguration WithUserAgent(string userAgent);
    }
}
