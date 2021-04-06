namespace StickersOnMap.WEB.Models
{
    using System;

    public class StickerDTO
    {
        public string Name { get; set; }
        public bool? Active { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}