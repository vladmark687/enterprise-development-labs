using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistrictStatisticsManagement.Models
{
    /// <summary>
    /// Класс, представляющий поставку сырья или комплектующих
    /// </summary>
    [Table("Supplies")]
    public class Supply
    {
        /// <summary>
        /// Идентификатор поставки (первичный ключ)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        /// <summary>
        /// Идентификатор предприятия-получателя (внешний ключ)
        /// </summary>
        [Required]
        [StringLength(50)]
        [ForeignKey("Enterprise")]
        public string EnterpriseRegistrationNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Предприятие-получатель (навигационное свойство)
        /// </summary>
        public Enterprise? Enterprise { get; set; }
        
        /// <summary>
        /// Идентификатор поставщика (внешний ключ)
        /// </summary>
        [Required]
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        
        /// <summary>
        /// Поставщик (навигационное свойство)
        /// </summary>
        public Supplier? Supplier { get; set; }
        
        /// <summary>
        /// Наименование товара/сырья
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// Количество единиц товара/сырья
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть положительным числом")]
        public int Quantity { get; set; }
        
        /// <summary>
        /// Дата поставки
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime SupplyDate { get; set; }
        
        /// <summary>
        /// Стоимость поставки
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть положительным числом")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
    }
}