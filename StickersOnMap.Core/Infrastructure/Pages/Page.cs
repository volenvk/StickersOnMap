using Newtonsoft.Json;

namespace StickersOnMap.Core.Infrastructure.Pages
{
    public class Page
    {
        [JsonProperty("size")]
        public int PageSize { get; set; }

        [JsonProperty("current")]
        public int PageNumber { get; set; }

        public override string ToString()
        {
            return $"PageSize: {PageSize}, PageNumber: {PageNumber}";
        }
    }
}