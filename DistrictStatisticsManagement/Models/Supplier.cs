using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistrictStatisticsManagement.Models
{
    /// <summary>
    /// Класс, представляющий поставщика
    /// </summary>
    [Table("Suppliers")]
    public class Supplier
    {
        /// <summary>
        /// Идентификатор поставщика (первичный ключ)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Адрес поставщика
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// Контактный телефон
        /// </summary>
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        /// <summary>
        /// Список поставок от данного поставщика
        /// </summary>
        public List<Supply> Supplies { get; set; } = new List<Supply>();
    }
}