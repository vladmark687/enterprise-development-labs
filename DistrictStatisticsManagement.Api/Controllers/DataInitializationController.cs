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

        // POST: api/DataInitialization/initialize
        [HttpPost("initialize")]
        public ActionResult InitializeTestData()
        {
            try
            {
                // Add enterprises
                var enterprise1 = new Enterprise { RegistrationNumber = "E001", Name = "ООО Стройматериалы", IndustryType = IndustryType.Construction, OwnershipType = OwnershipType.LimitedLiabilityPartnership };
                var enterprise2 = new Enterprise { RegistrationNumber = "E002", Name = "АО Металлопрокат", IndustryType = IndustryType.HeavyIndustry, OwnershipType = OwnershipType.JointStock };
                var enterprise3 = new Enterprise { RegistrationNumber = "E003", Name = "ИП Иванов", IndustryType = IndustryType.LightIndustry, OwnershipType = OwnershipType.Private };
                var enterprise4 = new Enterprise { RegistrationNumber = "E004", Name = "ООО Агрофирма", IndustryType = IndustryType.Agriculture, OwnershipType = OwnershipType.LimitedLiabilityPartnership };
                var enterprise5 = new Enterprise { RegistrationNumber = "E005", Name = "МУП ГорТранс", IndustryType = IndustryType.Transport, OwnershipType = OwnershipType.MunicipalCity };
                var enterprise6 = new Enterprise { RegistrationNumber = "E006", Name = "ООО СтройДом", IndustryType = IndustryType.Construction, OwnershipType = OwnershipType.LimitedLiabilityPartnership };
                var enterprise7 = new Enterprise { RegistrationNumber = "E007", Name = "ФКП Завод №5", IndustryType = IndustryType.HeavyIndustry, OwnershipType = OwnershipType.StateFederal };

                _repository.AddEnterprise(enterprise1);
                _repository.AddEnterprise(enterprise2);
                _repository.AddEnterprise(enterprise3);
                _repository.AddEnterprise(enterprise4);
                _repository.AddEnterprise(enterprise5);
                _repository.AddEnterprise(enterprise6);
                _repository.AddEnterprise(enterprise7);

                // Add suppliers
                var supplier1 = new Supplier { Id = 1, Name = "ООО МеталлТорг", Address = "г. Москва, ул. Промышленная, 1" };
                var supplier2 = new Supplier { Id = 2, Name = "АгроСырье", Address = "г. Краснодар, ул. Полевая, 15" };
                var supplier3 = new Supplier { Id = 3, Name = "ТрансЛогистик", Address = "г. Санкт-Петербург, пр. Транспортный, 7" };
                var supplier4 = new Supplier { Id = 4, Name = "СтройМаркет", Address = "г. Екатеринбург, ул. Строителей, 42" };
                var supplier5 = new Supplier { Id = 5, Name = "ТекстильПром", Address = "г. Иваново, ул. Ткацкая, 10" };

                _repository.AddSupplier(supplier1);
                _repository.AddSupplier(supplier2);
                _repository.AddSupplier(supplier3);
                _repository.AddSupplier(supplier4);
                _repository.AddSupplier(supplier5);

                // Add supplies
                var supply1 = new Supply { Id = 1, EnterpriseRegistrationNumber = "E002", SupplierId = 1, ProductName = "Стальной прокат", Quantity = 500, SupplyDate = new DateTime(2023, 1, 15) };
                var supply2 = new Supply { Id = 2, EnterpriseRegistrationNumber = "E004", SupplierId = 2, ProductName = "Семена пшеницы", Quantity = 1000, SupplyDate = new DateTime(2023, 2, 10) };
                var supply3 = new Supply { Id = 3, EnterpriseRegistrationNumber = "E005", SupplierId = 3, ProductName = "Запчасти для автобусов", Quantity = 150, SupplyDate = new DateTime(2023, 3, 5) };
                var supply4 = new Supply { Id = 4, EnterpriseRegistrationNumber = "E001", SupplierId = 4, ProductName = "Цемент", Quantity = 2000, SupplyDate = new DateTime(2023, 2, 20) };
                var supply5 = new Supply { Id = 5, EnterpriseRegistrationNumber = "E003", SupplierId = 5, ProductName = "Ткань", Quantity = 300, SupplyDate = new DateTime(2023, 1, 25) };
                var supply6 = new Supply { Id = 6, EnterpriseRegistrationNumber = "E006", SupplierId = 4, ProductName = "Кирпич", Quantity = 5000, SupplyDate = new DateTime(2023, 3, 15) };
                var supply7 = new Supply { Id = 7, EnterpriseRegistrationNumber = "E007", SupplierId = 1, ProductName = "Алюминиевый профиль", Quantity = 300, SupplyDate = new DateTime(2023, 4, 5) };
                var supply8 = new Supply { Id = 8, EnterpriseRegistrationNumber = "E002", SupplierId = 1, ProductName = "Медный прокат", Quantity = 200, SupplyDate = new DateTime(2023, 4, 10) };
                var supply9 = new Supply { Id = 9, EnterpriseRegistrationNumber = "E004", SupplierId = 2, ProductName = "Удобрения", Quantity = 1500, SupplyDate = new DateTime(2023, 4, 15) };
                var supply10 = new Supply { Id = 10, EnterpriseRegistrationNumber = "E001", SupplierId = 4, ProductName = "Песок", Quantity = 3000, SupplyDate = new DateTime(2023, 4, 20) };
                var supply11 = new Supply { Id = 11, EnterpriseRegistrationNumber = "E006", SupplierId = 4, ProductName = "Арматура", Quantity = 1000, SupplyDate = new DateTime(2023, 5, 5) };
                var supply12 = new Supply { Id = 12, EnterpriseRegistrationNumber = "E003", SupplierId = 5, ProductName = "Нитки", Quantity = 100, SupplyDate = new DateTime(2023, 5, 10) };
                var supply13 = new Supply { Id = 13, EnterpriseRegistrationNumber = "E005", SupplierId = 3, ProductName = "Шины", Quantity = 50, SupplyDate = new DateTime(2023, 5, 15) };
                var supply14 = new Supply { Id = 14, EnterpriseRegistrationNumber = "E007", SupplierId = 1, ProductName = "Титановый сплав", Quantity = 100, SupplyDate = new DateTime(2023, 5, 20) };
                var supply15 = new Supply { Id = 15, EnterpriseRegistrationNumber = "E002", SupplierId = 1, ProductName = "Чугун", Quantity = 800, SupplyDate = new DateTime(2023, 5, 25) };

                _repository.AddSupply(supply1);
                _repository.AddSupply(supply2);
                _repository.AddSupply(supply3);
                _repository.AddSupply(supply4);
                _repository.AddSupply(supply5);
                _repository.AddSupply(supply6);
                _repository.AddSupply(supply7);
                _repository.AddSupply(supply8);
                _repository.AddSupply(supply9);
                _repository.AddSupply(supply10);
                _repository.AddSupply(supply11);
                _repository.AddSupply(supply12);
                _repository.AddSupply(supply13);
                _repository.AddSupply(supply14);
                _repository.AddSupply(supply15);

                return Ok("Test data initialized successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error initializing test data: {ex.Message}");
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