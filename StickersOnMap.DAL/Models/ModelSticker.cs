using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StickersOnMap.DAL.Models
{
    using Core.Interfaces;
    
    public class ModelSticker : IEntity
    {
        [ForeignKey("Id")]
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [Column("Name"), NotNull]
        [MaxLength(200, ErrorMessage = "Не должно превышать 200 символов")]
        public string Name { get; set; }
        /// <summary>
        /// Широта
        /// </summary>
        [Column("Latitude"), NotNull]
        public float Latitude { get; set; }
        /// <summary>
        /// Долгота
        /// </summary>
        [Column("Longitude"), NotNull]
        public float Longitude { get; set; }
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