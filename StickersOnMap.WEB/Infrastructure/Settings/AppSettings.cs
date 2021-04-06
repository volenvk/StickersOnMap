namespace StickersOnMap.WEB.Infrastructure.Settings
{
    using System;
    using Microsoft.Extensions.Configuration;

    public class AppSettings : IAppSettings
    {
        public string UriMap { get; set; }
        
        public AppSettings(IConfiguration configuration)
        {
            var conf = configuration ?? throw  new ArgumentNullException(nameof(configuration));

            try
            {
                UriMap = conf[nameof(UriMap)];
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "Параметры настроек приложения не определены", ex);
            }
        }
    }
}