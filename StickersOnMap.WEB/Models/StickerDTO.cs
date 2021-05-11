namespace StickersOnMap.WEB.Models
{
    using System;

    /// <summary>
    /// Стикер для UI
    /// </summary>
    public class StickerDTO
    {
        /// <summary>
        /// Идентификационный номер в БД
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Отобрадать на карте
        /// </summary>
        public bool? Active { get; set; }
        
        /// <summary>
        /// Дата и время создание стикера
        /// </summary>
        public DateTime? CreateDate { get; set; }
    }
}