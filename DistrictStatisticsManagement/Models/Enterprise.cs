using DistrictStatisticsManagement.Enums;

namespace DistrictStatisticsManagement.Models
{
    /// <summary>
    /// Класс, представляющий предприятие
    /// </summary>
    public class Enterprise
    {
        /// <summary>
        /// Регистрационный номер предприятия
        /// </summary>
        public string RegistrationNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Тип отрасли
        /// </summary>
        public IndustryType IndustryType { get; set; }
        
        /// <summary>
        /// Наименование предприятия
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Адрес предприятия
        /// </summary>
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// Телефон предприятия
        /// </summary>
        public string Phone { get; set; } = string.Empty;
        
        /// <summary>
        /// Форма собственности
        /// </summary>
        public OwnershipType OwnershipType { get; set; }
        
        /// <summary>
        /// Количество работающих
        /// </summary>
        public int EmployeeCount { get; set; }
        
        /// <summary>
        /// Общая площадь
        /// </summary>
        public double TotalArea { get; set; }
        
        /// <summary>
        /// Список поставок для данного предприятия
        /// </summary>
        public List<Supply> Supplies { get; set; } = new List<Supply>();
    }
}