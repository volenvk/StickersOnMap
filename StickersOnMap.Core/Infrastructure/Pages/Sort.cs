using Newtonsoft.Json;

namespace StickersOnMap.Core.Infrastructure.Pages
{
    public class Sort
    {
        [JsonProperty("by")]
        public string Property { get; set; }

        public bool Reverse { get; set; }

        public override string ToString()
        {
            return $"Property: {Property}, Reverse: {Reverse}";
        }
    }
}