namespace DistrictStatisticsManagement.Models
{
    /// <summary>
    /// Класс, представляющий поставку сырья или комплектующих
    /// </summary>
    public class Supply
    {
        /// <summary>
        /// Идентификатор поставки
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Идентификатор предприятия-получателя
        /// </summary>
        public string EnterpriseRegistrationNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Предприятие-получатель
        /// </summary>
        public Enterprise? Enterprise { get; set; }
        
        /// <summary>
        /// Идентификатор поставщика
        /// </summary>
        public int SupplierId { get; set; }
        
        /// <summary>
        /// Поставщик
        /// </summary>
        public Supplier? Supplier { get; set; }
        
        /// <summary>
        /// Наименование товара/сырья
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// Количество единиц товара/сырья
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Дата поставки
        /// </summary>
        public DateTime SupplyDate { get; set; }
    }
}