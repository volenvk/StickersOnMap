namespace StickersOnMap.Core.Infrastructure.Filteres
{
    public class Filter
    {
        public string Property { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Property: {Property}, Value: {Value}";
        }
    }
}