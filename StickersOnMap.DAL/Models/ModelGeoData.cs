namespace StickersOnMap.DAL.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using Core.Interfaces;

    public class ModelGeoData : IEntry
    {
        [ForeignKey("ID")]
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [Column("Name"), NotNull]
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