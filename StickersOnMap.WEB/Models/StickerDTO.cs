namespace StickersOnMap.WEB.Models
{
    using System;

    public class StickerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}