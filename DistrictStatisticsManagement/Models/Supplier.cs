namespace DistrictStatisticsManagement.Models
{
    /// <summary>
    /// Класс, представляющий поставщика
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// Идентификатор поставщика
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Адрес поставщика
        /// </summary>
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// Контактный телефон
        /// </summary>
        public string Phone { get; set; } = string.Empty;
        
        /// <summary>
        /// Список поставок от данного поставщика
        /// </summary>
        public List<Supply> Supplies { get; set; } = new List<Supply>();
    }
}