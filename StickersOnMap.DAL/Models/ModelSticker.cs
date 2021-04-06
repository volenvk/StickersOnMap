using System;

namespace StickersOnMap.DAL.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using Core.Interfaces;
    
    public class ModelSticker : IEntry
    {
        [ForeignKey("ID")]
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [Column("Name"), NotNull]
        public string Name { get; set; }
        /// <summary>
        /// Отображение на карте
        /// </summary>
        [Column("Active"), NotNull]
        public bool Active { get; set; }
        /// <summary>
        /// Время создания записи
        /// </summary>
        [Column("CreateDate"), NotNull]
        public DateTime CreateDate { get; set; }
    }
}