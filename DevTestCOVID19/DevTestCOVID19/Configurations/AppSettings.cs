using Microsoft.Extensions.Configuration;

namespace DevTestCOVID19.Configurations
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;
        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BaseAPIURL
        {
            get
            {
                return _configuration.GetValue<string>("AppSettings:BaseAPIURL");
            }
        }

        public string XRapidAPIHost
        {
            get
            {
                return _configuration.GetValue<string>("AppSettings:XRapidAPIHost");
            }
        }

        public string XRapidAPIKey
        {
            get
            {
                return _configuration.GetValue<string>("AppSettings:XRapidAPIKey");
            }
        }
    }
}
