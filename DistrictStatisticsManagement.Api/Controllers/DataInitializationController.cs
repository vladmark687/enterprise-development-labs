using DistrictStatisticsManagement.Enums;
using DistrictStatisticsManagement.Models;
using DistrictStatisticsManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DistrictStatisticsManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataInitializationController : ControllerBase
    {
        private readonly DistrictStatisticsRepository _repository;

        public DataInitializationController(DistrictStatisticsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Инициализация базы данных тестовыми данными (минимум 10 записей для каждой таблицы)
        /// </summary>
        [HttpPost("initialize")]
        public ActionResult InitializeTestData()
        {
            try
            {
                // Очистка существующих данных
                _repository.RemoveAllSupplies();
                _repository.RemoveAllSuppliers();
                _repository.RemoveAllEnterprises();

                // Добавление предприятий (15 записей)
                var enterprises = new List<Enterprise>
                 {
                     new Enterprise { RegistrationNumber = "REG001", Name = "ООО \"Металлургический завод\"", IndustryType = IndustryType.HeavyIndustry, Address = "г. Москва, ул. Промышленная, 10", Phone = "+7-495-123-45-67", OwnershipType = OwnershipType.Private, EmployeeCount = 1500, TotalArea = 25000.50 },
                     new Enterprise { RegistrationNumber = "REG002", Name = "АО \"Пищевая компания\"", IndustryType = IndustryType.LightIndustry, Address = "г. Санкт-Петербург, пр. Невский, 100", Phone = "+7-812-234-56-78", OwnershipType = OwnershipType.JointStock, EmployeeCount = 250, TotalArea = 15000.75 },
                     new Enterprise { RegistrationNumber = "REG003", Name = "ИП \"Текстильная фабрика\"", IndustryType = IndustryType.LightIndustry, Address = "г. Иваново, ул. Текстильщиков, 25", Phone = "+7-493-345-67-89", OwnershipType = OwnershipType.Private, EmployeeCount = 180, TotalArea = 12000.00 },
                     new Enterprise { RegistrationNumber = "REG004", Name = "ООО \"Химический комбинат\"", IndustryType = IndustryType.HeavyIndustry, Address = "г. Тула, ул. Химиков, 50", Phone = "+7-487-456-78-90", OwnershipType = OwnershipType.Private, EmployeeCount = 320, TotalArea = 18500.25 },
                     new Enterprise { RegistrationNumber = "REG005", Name = "АО \"Машиностроительный завод\"", IndustryType = IndustryType.HeavyIndustry, Address = "г. Нижний Новгород, ул. Заводская, 15", Phone = "+7-831-567-89-01", OwnershipType = OwnershipType.JointStock, EmployeeCount = 450, TotalArea = 22000.80 },
                     new Enterprise { RegistrationNumber = "REG006", Name = "ООО \"Строительная компания\"", IndustryType = IndustryType.Construction, Address = "г. Екатеринбург, ул. Строителей, 30", Phone = "+7-343-678-90-12", OwnershipType = OwnershipType.Private, EmployeeCount = 200, TotalArea = 8000.00 },
                     new Enterprise { RegistrationNumber = "REG007", Name = "ИП \"Транспортная фирма\"", IndustryType = IndustryType.Transport, Address = "г. Новосибирск, ул. Транспортная, 45", Phone = "+7-383-789-01-23", OwnershipType = OwnershipType.Private, EmployeeCount = 150, TotalArea = 5500.50 },
                     new Enterprise { RegistrationNumber = "REG008", Name = "АО \"Энергетическая компания\"", IndustryType = IndustryType.HeavyIndustry, Address = "г. Казань, ул. Энергетиков, 20", Phone = "+7-843-890-12-34", OwnershipType = OwnershipType.JointStock, EmployeeCount = 380, TotalArea = 30000.00 },
                     new Enterprise { RegistrationNumber = "REG009", Name = "ООО \"IT-Решения\"", IndustryType = IndustryType.LightIndustry, Address = "г. Москва, ул. Программистов, 10", Phone = "+7-495-901-23-45", OwnershipType = OwnershipType.Private, EmployeeCount = 120, TotalArea = 3000.25 },
                     new Enterprise { RegistrationNumber = "REG010", Name = "ИП \"Сельхозпредприятие\"", IndustryType = IndustryType.Agriculture, Address = "г. Краснодар, ул. Полевая, 60", Phone = "+7-861-012-34-56", OwnershipType = OwnershipType.Private, EmployeeCount = 80, TotalArea = 50000.00 },
                     new Enterprise { RegistrationNumber = "REG011", Name = "ООО \"Фармацевтическая компания\"", IndustryType = IndustryType.LightIndustry, Address = "г. Москва, ул. Медицинская, 35", Phone = "+7-495-123-98-76", OwnershipType = OwnershipType.Private, EmployeeCount = 220, TotalArea = 8500.75 },
                     new Enterprise { RegistrationNumber = "REG012", Name = "АО \"Автомобильный завод\"", IndustryType = IndustryType.HeavyIndustry, Address = "г. Тольятти, ул. Автозаводская, 1", Phone = "+7-848-234-87-65", OwnershipType = OwnershipType.JointStock, EmployeeCount = 600, TotalArea = 40000.00 },
                     new Enterprise { RegistrationNumber = "REG013", Name = "ООО \"Мебельная фабрика\"", IndustryType = IndustryType.LightIndustry, Address = "г. Воронеж, ул. Мебельщиков, 18", Phone = "+7-473-345-76-54", OwnershipType = OwnershipType.Private, EmployeeCount = 160, TotalArea = 9500.50 },
                     new Enterprise { RegistrationNumber = "REG014", Name = "ИП \"Рыбоперерабатывающий завод\"", IndustryType = IndustryType.LightIndustry, Address = "г. Мурманск, ул. Рыбацкая, 22", Phone = "+7-815-456-65-43", OwnershipType = OwnershipType.Private, EmployeeCount = 90, TotalArea = 6000.25 },
                     new Enterprise { RegistrationNumber = "REG015", Name = "АО \"Нефтехимический комбинат\"", IndustryType = IndustryType.HeavyIndustry, Address = "г. Уфа, ул. Нефтяников, 75", Phone = "+7-347-567-54-32", OwnershipType = OwnershipType.JointStock, EmployeeCount = 750, TotalArea = 60000.00 }
                 };

                foreach (var enterprise in enterprises)
                {
                    _repository.AddEnterprise(enterprise);
                }

                // Добавление поставщиков (12 записей)
                var suppliers = new List<Supplier>
                {
                    new Supplier { Name = "ООО \"Поставщик Металлов\"", Address = "г. Челябинск, ул. Металлургов, 12", Phone = "+7-351-111-22-33" },
                    new Supplier { Name = "АО \"Продукты Питания\"", Address = "г. Ростов-на-Дону, ул. Пищевая, 8", Phone = "+7-863-222-33-44" },
                    new Supplier { Name = "ИП \"Текстильные Материалы\"", Address = "г. Иваново, ул. Ткацкая, 5", Phone = "+7-493-333-44-55" },
                    new Supplier { Name = "ООО \"Химические Реактивы\"", Address = "г. Пермь, ул. Химическая, 28", Phone = "+7-342-444-55-66" },
                    new Supplier { Name = "АО \"Машинные Детали\"", Address = "г. Самара, ул. Механическая, 40", Phone = "+7-846-555-66-77" },
                    new Supplier { Name = "ООО \"Стройматериалы Плюс\"", Address = "г. Волгоград, ул. Строительная, 33", Phone = "+7-844-666-77-88" },
                    new Supplier { Name = "ИП \"Логистические Услуги\"", Address = "г. Красноярск, ул. Транспортная, 17", Phone = "+7-391-777-88-99" },
                    new Supplier { Name = "АО \"Энергоресурсы\"", Address = "г. Омск, ул. Энергетическая, 55", Phone = "+7-381-888-99-00" },
                    new Supplier { Name = "ООО \"Компьютерные Технологии\"", Address = "г. Санкт-Петербург, ул. IT-парк, 7", Phone = "+7-812-999-00-11" },
                    new Supplier { Name = "ИП \"Агропоставки\"", Address = "г. Ставрополь, ул. Сельская, 42", Phone = "+7-865-000-11-22" },
                    new Supplier { Name = "АО \"Медицинское Оборудование\"", Address = "г. Москва, ул. Медтехника, 25", Phone = "+7-495-111-33-55" },
                    new Supplier { Name = "ООО \"Автокомплектующие\"", Address = "г. Нижний Новгород, ул. Автозапчастей, 14", Phone = "+7-831-222-44-66" }
                };

                foreach (var supplier in suppliers)
                 {
                     _repository.AddSupplier(supplier);
                 }

                 // Добавление поставок (20 записей)
                 var supplies = new List<Supply>
                 {
                     new Supply { EnterpriseRegistrationNumber = "REG001", SupplierId = 1, ProductName = "Сталь листовая", Quantity = 1000, Cost = 50000.00m, SupplyDate = new DateTime(2024, 1, 15) },
                     new Supply { EnterpriseRegistrationNumber = "REG001", SupplierId = 1, ProductName = "Арматура стальная", Quantity = 500, Cost = 25000.00m, SupplyDate = new DateTime(2024, 2, 20) },
                     new Supply { EnterpriseRegistrationNumber = "REG002", SupplierId = 2, ProductName = "Мука пшеничная", Quantity = 2000, Cost = 80000.00m, SupplyDate = new DateTime(2024, 1, 10) },
                     new Supply { EnterpriseRegistrationNumber = "REG002", SupplierId = 2, ProductName = "Сахар-песок", Quantity = 1500, Cost = 90000.00m, SupplyDate = new DateTime(2024, 3, 5) },
                     new Supply { EnterpriseRegistrationNumber = "REG003", SupplierId = 3, ProductName = "Хлопковая пряжа", Quantity = 800, Cost = 120000.00m, SupplyDate = new DateTime(2024, 1, 25) },
                     new Supply { EnterpriseRegistrationNumber = "REG003", SupplierId = 3, ProductName = "Синтетические волокна", Quantity = 600, Cost = 75000.00m, SupplyDate = new DateTime(2024, 2, 15) },
                     new Supply { EnterpriseRegistrationNumber = "REG004", SupplierId = 4, ProductName = "Серная кислота", Quantity = 300, Cost = 45000.00m, SupplyDate = new DateTime(2024, 1, 30) },
                     new Supply { EnterpriseRegistrationNumber = "REG004", SupplierId = 4, ProductName = "Катализаторы", Quantity = 100, Cost = 150000.00m, SupplyDate = new DateTime(2024, 3, 10) },
                     new Supply { EnterpriseRegistrationNumber = "REG005", SupplierId = 5, ProductName = "Подшипники", Quantity = 2000, Cost = 200000.00m, SupplyDate = new DateTime(2024, 2, 1) },
                     new Supply { EnterpriseRegistrationNumber = "REG005", SupplierId = 5, ProductName = "Электродвигатели", Quantity = 50, Cost = 500000.00m, SupplyDate = new DateTime(2024, 3, 15) },
                     new Supply { EnterpriseRegistrationNumber = "REG006", SupplierId = 6, ProductName = "Цемент", Quantity = 5000, Cost = 250000.00m, SupplyDate = new DateTime(2024, 1, 20) },
                     new Supply { EnterpriseRegistrationNumber = "REG006", SupplierId = 6, ProductName = "Кирпич строительный", Quantity = 10000, Cost = 300000.00m, SupplyDate = new DateTime(2024, 2, 25) },
                     new Supply { EnterpriseRegistrationNumber = "REG007", SupplierId = 7, ProductName = "Автомобильные шины", Quantity = 200, Cost = 400000.00m, SupplyDate = new DateTime(2024, 1, 12) },
                     new Supply { EnterpriseRegistrationNumber = "REG007", SupplierId = 7, ProductName = "Топливо дизельное", Quantity = 1000, Cost = 60000.00m, SupplyDate = new DateTime(2024, 3, 8) },
                     new Supply { EnterpriseRegistrationNumber = "REG008", SupplierId = 8, ProductName = "Турбины", Quantity = 5, Cost = 2000000.00m, SupplyDate = new DateTime(2024, 2, 10) },
                     new Supply { EnterpriseRegistrationNumber = "REG008", SupplierId = 8, ProductName = "Кабель силовой", Quantity = 2000, Cost = 180000.00m, SupplyDate = new DateTime(2024, 3, 20) },
                     new Supply { EnterpriseRegistrationNumber = "REG009", SupplierId = 9, ProductName = "Серверы", Quantity = 20, Cost = 800000.00m, SupplyDate = new DateTime(2024, 1, 18) },
                     new Supply { EnterpriseRegistrationNumber = "REG009", SupplierId = 9, ProductName = "Программное обеспечение", Quantity = 100, Cost = 1200000.00m, SupplyDate = new DateTime(2024, 2, 28) },
                     new Supply { EnterpriseRegistrationNumber = "REG010", SupplierId = 10, ProductName = "Семена пшеницы", Quantity = 1000, Cost = 35000.00m, SupplyDate = new DateTime(2024, 1, 5) },
                     new Supply { EnterpriseRegistrationNumber = "REG010", SupplierId = 10, ProductName = "Удобрения минеральные", Quantity = 500, Cost = 85000.00m, SupplyDate = new DateTime(2024, 3, 12) }
                 };

                 foreach (var supply in supplies)
                 {
                     _repository.AddSupply(supply);
                 }

                 return Ok(new
                 {
                     message = "База данных успешно инициализирована",
                     enterprises_count = enterprises.Count,
                     suppliers_count = suppliers.Count,
                     supplies_count = supplies.Count,
                     total_records = enterprises.Count + suppliers.Count + supplies.Count
                 });
             }
             catch (Exception ex)
             {
                 return BadRequest(new { error = "Ошибка при инициализации данных", details = ex.Message });
             }
         }

         /// <summary>
         /// Получить статистику по количеству записей в таблицах
         /// </summary>
         [HttpGet("statistics")]
         public IActionResult GetDataStatistics()
         {
             try
             {
                 var enterprisesCount = _repository.GetAllEnterprises().Count();
                 var suppliersCount = _repository.GetAllSuppliers().Count();
                 var suppliesCount = _repository.GetAllSupplies().Count();

                 return Ok(new
                 {
                     enterprises_count = enterprisesCount,
                     suppliers_count = suppliersCount,
                     supplies_count = suppliesCount,
                     total_records = enterprisesCount + suppliersCount + suppliesCount,
                     last_updated = DateTime.Now
                 });
             }
             catch (Exception ex)
             {
                 return BadRequest(new { error = "Ошибка при получении статистики", details = ex.Message });
             }
         }

         /// <summary>
         /// Очистить все данные из таблиц
         /// </summary>
         [HttpDelete("clear-all")]
         public IActionResult ClearAllData()
         {
             try
             {
                 _repository.RemoveAllSupplies();
                 _repository.RemoveAllSuppliers();
                 _repository.RemoveAllEnterprises();

                 return Ok(new { message = "Все данные успешно удалены" });
             }
             catch (Exception ex)
             {
                 return BadRequest(new { error = "Ошибка при очистке данных", details = ex.Message });
             }
         }

         // POST: api/DataInitialization/clear
         [HttpPost("clear")]
        public ActionResult ClearData()
        {
            try
            {
                _repository.ClearData();
                return Ok("All data cleared successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error clearing data: {ex.Message}");
            }
        }
    }
}