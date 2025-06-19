using DistrictStatisticsManagement.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DistrictStatisticsManagement.Models
{
    /// <summary>
    /// Класс, представляющий предприятие
    /// </summary>
    [Table("Enterprises")]
    public class Enterprise
    {
        /// <summary>
        /// Регистрационный номер предприятия (первичный ключ)
        /// </summary>
        [Key]
        [Required]
        [StringLength(50)]
        public string RegistrationNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Тип отрасли
        /// </summary>
        [Required]
        public IndustryType IndustryType { get; set; }
        
        /// <summary>
        /// Наименование предприятия
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Адрес предприятия
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// Телефон предприятия
        /// </summary>
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        /// <summary>
        /// Форма собственности
        /// </summary>
        [Required]
        public OwnershipType OwnershipType { get; set; }
        
        /// <summary>
        /// Количество работающих
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество работающих не может быть отрицательным")]
        public int EmployeeCount { get; set; }
        
        /// <summary>
        /// Общая площадь (в квадратных метрах)
        /// </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Общая площадь не может быть отрицательной")]
        [Column(TypeName = "decimal(18,2)")]
        public double TotalArea { get; set; }
        
        /// <summary>
        /// Список поставок для данного предприятия
        /// </summary>
        [JsonIgnore]
        public List<Supply> Supplies { get; set; } = new List<Supply>();
    }
}