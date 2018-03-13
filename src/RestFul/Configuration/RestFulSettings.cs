namespace RestFul.Configuration
{
    public class RestFulSettings : IRestFulSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseHTTPs { get; set; }  

        public RestFulSettings()
        {
            Host = "localhost";
            Port = 8000;
            UseHTTPs = false;
        }

        public IRestFulSettings WithHost(string host)
        {
            Host = host;
            return this;
        }

        public IRestFulSettings WithHttps()
        {
            UseHTTPs = true;
            return this;
        }

        public IRestFulSettings WithPort(int port)
        {
            Port = port;
            return this;
        }
    }
}
