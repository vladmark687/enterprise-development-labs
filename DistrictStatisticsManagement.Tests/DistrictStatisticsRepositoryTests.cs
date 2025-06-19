using DistrictStatisticsManagement.Data;
using DistrictStatisticsManagement.Enums;
using DistrictStatisticsManagement.Models;
using DistrictStatisticsManagement.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DistrictStatisticsManagement.Tests
{
    public class DistrictStatisticsRepositoryTests
    {
        private readonly DistrictStatisticsRepository _repository;
        private readonly AppDbContext _context;

        public DistrictStatisticsRepositoryTests()
        {
            // Создаем опции для in-memory базы данных
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDistrictStatistics")
                .Options;

            // Создаем контекст с in-memory базой данных
            _context = new AppDbContext(options);
            
            // Очищаем базу данных перед каждым тестом
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            
            // Инициализация репозитория и заполнение тестовыми данными
            _repository = new DistrictStatisticsRepository(_context);
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // Создание предприятий
            var enterprises = new List<Enterprise>
            {
                new Enterprise
                {
                    RegistrationNumber = "E001",
                    Name = "Агрокомплекс Заря",
                    IndustryType = IndustryType.Agriculture,
                    Address = "ул. Полевая, 10",
                    Phone = "8-800-100-1001",
                    OwnershipType = OwnershipType.StateFederal,
                    EmployeeCount = 150,
                    TotalArea = 5000.5
                },
                new Enterprise
                {
                    RegistrationNumber = "E002",
                    Name = "ТрансАвто",
                    IndustryType = IndustryType.Transport,
                    Address = "ул. Транспортная, 25",
                    Phone = "8-800-100-1002",
                    OwnershipType = OwnershipType.Private,
                    EmployeeCount = 80,
                    TotalArea = 1200.0
                },
                new Enterprise
                {
                    RegistrationNumber = "E003",
                    Name = "ЛегПром Текстиль",
                    IndustryType = IndustryType.LightIndustry,
                    Address = "ул. Фабричная, 5",
                    Phone = "8-800-100-1003",
                    OwnershipType = OwnershipType.JointStock,
                    EmployeeCount = 200,
                    TotalArea = 3000.0
                },
                new Enterprise
                {
                    RegistrationNumber = "E004",
                    Name = "МеталлСтрой",
                    IndustryType = IndustryType.HeavyIndustry,
                    Address = "ул. Заводская, 15",
                    Phone = "8-800-100-1004",
                    OwnershipType = OwnershipType.JointStock,
                    EmployeeCount = 300,
                    TotalArea = 8000.0
                },
                new Enterprise
                {
                    RegistrationNumber = "E005",
                    Name = "СтройГрад",
                    IndustryType = IndustryType.Construction,
                    Address = "ул. Строителей, 30",
                    Phone = "8-800-100-1005",
                    OwnershipType = OwnershipType.LimitedLiabilityPartnership,
                    EmployeeCount = 120,
                    TotalArea = 2500.0
                },
                new Enterprise
                {
                    RegistrationNumber = "E006",
                    Name = "ТехСнаб",
                    IndustryType = IndustryType.MaterialSupply,
                    Address = "ул. Складская, 8",
                    Phone = "8-800-100-1006",
                    OwnershipType = OwnershipType.MunicipalCity,
                    EmployeeCount = 50,
                    TotalArea = 1800.0
                },
                new Enterprise
                {
                    RegistrationNumber = "E007",
                    Name = "АгроФерма Рассвет",
                    IndustryType = IndustryType.Agriculture,
                    Address = "ул. Сельская, 12",
                    Phone = "8-800-100-1007",
                    OwnershipType = OwnershipType.Private,
                    EmployeeCount = 90,
                    TotalArea = 4000.0
                }
            };

            // Добавление предприятий в контекст базы данных
            _context.Enterprises.AddRange(enterprises);

            // Создание поставщиков
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    Id = 1,
                    Name = "СырьеПром",
                    Address = "ул. Промышленная, 20",
                    Phone = "8-800-200-2001"
                },
                new Supplier
                {
                    Id = 2,
                    Name = "АгроСырье",
                    Address = "ул. Аграрная, 15",
                    Phone = "8-800-200-2002"
                },
                new Supplier
                {
                    Id = 3,
                    Name = "МеталлСнаб",
                    Address = "ул. Металлургов, 10",
                    Phone = "8-800-200-2003"
                },
                new Supplier
                {
                    Id = 4,
                    Name = "ТекстильПром",
                    Address = "ул. Текстильщиков, 5",
                    Phone = "8-800-200-2004"
                },
                new Supplier
                {
                    Id = 5,
                    Name = "СтройМатериалы",
                    Address = "ул. Строительная, 25",
                    Phone = "8-800-200-2005"
                }
            };

            // Добавление поставщиков в контекст базы данных
            _context.Suppliers.AddRange(suppliers);

            // Создание поставок
            var supplies = new List<Supply>
            {
                new Supply
                {
                    Id = 1,
                    EnterpriseRegistrationNumber = "E001",
                    SupplierId = 2,
                    ProductName = "Семена пшеницы",
                    Quantity = 1000,
                    SupplyDate = new DateTime(2023, 1, 15)
                },
                new Supply
                {
                    Id = 2,
                    EnterpriseRegistrationNumber = "E001",
                    SupplierId = 2,
                    ProductName = "Удобрения",
                    Quantity = 500,
                    SupplyDate = new DateTime(2023, 2, 10)
                },
                new Supply
                {
                    Id = 3,
                    EnterpriseRegistrationNumber = "E002",
                    SupplierId = 1,
                    ProductName = "Запчасти для автомобилей",
                    Quantity = 200,
                    SupplyDate = new DateTime(2023, 1, 20)
                },
                new Supply
                {
                    Id = 4,
                    EnterpriseRegistrationNumber = "E003",
                    SupplierId = 4,
                    ProductName = "Ткань",
                    Quantity = 3000,
                    SupplyDate = new DateTime(2023, 3, 5)
                },
                new Supply
                {
                    Id = 5,
                    EnterpriseRegistrationNumber = "E004",
                    SupplierId = 3,
                    ProductName = "Металлопрокат",
                    Quantity = 5000,
                    SupplyDate = new DateTime(2023, 2, 15)
                },
                new Supply
                {
                    Id = 6,
                    EnterpriseRegistrationNumber = "E004",
                    SupplierId = 3,
                    ProductName = "Стальные листы",
                    Quantity = 2000,
                    SupplyDate = new DateTime(2023, 3, 10)
                },
                new Supply
                {
                    Id = 7,
                    EnterpriseRegistrationNumber = "E005",
                    SupplierId = 5,
                    ProductName = "Цемент",
                    Quantity = 1500,
                    SupplyDate = new DateTime(2023, 1, 25)
                },
                new Supply
                {
                    Id = 8,
                    EnterpriseRegistrationNumber = "E005",
                    SupplierId = 5,
                    ProductName = "Кирпич",
                    Quantity = 10000,
                    SupplyDate = new DateTime(2023, 2, 20)
                },
                new Supply
                {
                    Id = 9,
                    EnterpriseRegistrationNumber = "E006",
                    SupplierId = 1,
                    ProductName = "Комплектующие",
                    Quantity = 800,
                    SupplyDate = new DateTime(2023, 3, 15)
                },
                new Supply
                {
                    Id = 10,
                    EnterpriseRegistrationNumber = "E007",
                    SupplierId = 2,
                    ProductName = "Корма для животных",
                    Quantity = 2000,
                    SupplyDate = new DateTime(2023, 1, 10)
                },
                new Supply
                {
                    Id = 11,
                    EnterpriseRegistrationNumber = "E001",
                    SupplierId = 1,
                    ProductName = "Сельхозтехника",
                    Quantity = 5,
                    SupplyDate = new DateTime(2023, 3, 20)
                },
                new Supply
                {
                    Id = 12,
                    EnterpriseRegistrationNumber = "E003",
                    SupplierId = 4,
                    ProductName = "Нитки",
                    Quantity = 500,
                    SupplyDate = new DateTime(2023, 2, 25)
                },
                new Supply
                {
                    Id = 13,
                    EnterpriseRegistrationNumber = "E002",
                    SupplierId = 3,
                    ProductName = "Металлические детали",
                    Quantity = 300,
                    SupplyDate = new DateTime(2023, 3, 25)
                },
                new Supply
                {
                    Id = 14,
                    EnterpriseRegistrationNumber = "E006",
                    SupplierId = 5,
                    ProductName = "Строительные материалы",
                    Quantity = 1200,
                    SupplyDate = new DateTime(2023, 1, 5)
                },
                new Supply
                {
                    Id = 15,
                    EnterpriseRegistrationNumber = "E007",
                    SupplierId = 2,
                    ProductName = "Семена кукурузы",
                    Quantity = 800,
                    SupplyDate = new DateTime(2023, 2, 5)
                }
            };

            // Добавление поставок в контекст базы данных
            _context.Supplies.AddRange(supplies);
            
            // Сохранение изменений в базе данных
            _context.SaveChanges();
        }

        [Fact]
        public void Test1_GetEnterpriseByRegistrationNumber()
        {
            // Arrange
            string registrationNumber = "E001";

            // Act
            var enterprise = _repository.GetEnterpriseByRegistrationNumber(registrationNumber);

            // Assert
            Assert.NotNull(enterprise);
            Assert.Equal(registrationNumber, enterprise.RegistrationNumber);
            Assert.Equal("Агрокомплекс Заря", enterprise.Name);
            Assert.Equal(IndustryType.Agriculture, enterprise.IndustryType);
            Assert.Equal(OwnershipType.StateFederal, enterprise.OwnershipType);
            Assert.Equal(150, enterprise.EmployeeCount);
            Assert.Equal(5000.5, enterprise.TotalArea);
        }

        [Fact]
        public void Test2_GetSuppliersByPeriod()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 31);

            // Act
            var suppliers = _repository.GetSuppliersByPeriod(startDate, endDate).ToList();

            // Assert
            Assert.Equal(3, suppliers.Count); // Должно быть 3 уникальных поставщика за январь
            Assert.Contains(suppliers, s => s.Name == "АгроСырье");
            Assert.Contains(suppliers, s => s.Name == "СтройМатериалы");
            Assert.Contains(suppliers, s => s.Name == "СырьеПром");

            // Проверка сортировки по названию
            Assert.Equal("АгроСырье", suppliers[0].Name);
            Assert.Equal("СтройМатериалы", suppliers[1].Name);
            Assert.Equal("СырьеПром", suppliers[2].Name);
        }

        [Fact]
        public void Test3_GetEnterpriseCountBySupplier()
        {
            // Act
            var result = _repository.GetEnterpriseCountBySupplier();

            // Assert
            Assert.Equal(5, result.Count); // Всего 5 поставщиков

            // Выводим для отладки фактические значения
            var supplierCounts = result.ToDictionary(kv => kv.Key.Name, kv => kv.Value);
            
            // Проверяем количество предприятий для каждого поставщика
            // Поставщик 1 (СырьеПром) работает с предприятиями E001, E002, E006 = 3
            Assert.Equal(3, supplierCounts["СырьеПром"]); 
            // Поставщик 2 (АгроСырье) работает с предприятиями E001, E007 = 2
            Assert.Equal(2, supplierCounts["АгроСырье"]); 
            // Поставщик 3 (МеталлСнаб) работает с предприятиями E002, E004 = 2
            Assert.Equal(2, supplierCounts["МеталлСнаб"]); 
            // Поставщик 4 (ТекстильПром) работает с предприятием E003 = 1
            Assert.Equal(1, supplierCounts["ТекстильПром"]); 
            // Поставщик 5 (СтройМатериалы) работает с предприятиями E005, E006 = 2
            Assert.Equal(2, supplierCounts["СтройМатериалы"]); 
        }

        [Fact]
        public void Test4_GetSupplierCountByIndustryAndOwnership()
        {
            // Act
            var result = _repository.GetSupplierCountByIndustryAndOwnership();

            // Assert
            Assert.True(result.ContainsKey((IndustryType.Agriculture, OwnershipType.StateFederal)));
            Assert.True(result.ContainsKey((IndustryType.Agriculture, OwnershipType.Private)));
            Assert.True(result.ContainsKey((IndustryType.Transport, OwnershipType.Private)));
            Assert.True(result.ContainsKey((IndustryType.LightIndustry, OwnershipType.JointStock)));
            Assert.True(result.ContainsKey((IndustryType.HeavyIndustry, OwnershipType.JointStock)));
            Assert.True(result.ContainsKey((IndustryType.Construction, OwnershipType.LimitedLiabilityPartnership)));
            Assert.True(result.ContainsKey((IndustryType.MaterialSupply, OwnershipType.MunicipalCity)));

            // Проверка количества поставщиков для некоторых комбинаций
            Assert.Equal(2, result[(IndustryType.Agriculture, OwnershipType.StateFederal)]); // Агрокомплекс Заря имеет 2 поставщика
            Assert.Equal(1, result[(IndustryType.Agriculture, OwnershipType.Private)]); // АгроФерма Рассвет имеет 1 поставщика
            Assert.Equal(2, result[(IndustryType.Transport, OwnershipType.Private)]); // ТрансАвто имеет 2 поставщика
            Assert.Equal(1, result[(IndustryType.LightIndustry, OwnershipType.JointStock)]); // ЛегПром Текстиль имеет 1 поставщика
            Assert.Equal(1, result[(IndustryType.HeavyIndustry, OwnershipType.JointStock)]); // МеталлСтрой имеет 1 поставщика
            Assert.Equal(1, result[(IndustryType.Construction, OwnershipType.LimitedLiabilityPartnership)]); // СтройГрад имеет 1 поставщика
            Assert.Equal(2, result[(IndustryType.MaterialSupply, OwnershipType.MunicipalCity)]); // ТехСнаб имеет 2 поставщика
        }

        [Fact]
        public void Test5_GetTop5EnterprisesBySupplyCount()
        {
            // Act
            var topEnterprises = _repository.GetTop5EnterprisesBySupplyCount().ToList();

            // Assert
            Assert.Equal(5, topEnterprises.Count); // Должно быть 5 предприятий

            // Выводим для отладки фактические значения и количество поставок
            var enterprisesWithCounts = topEnterprises.Select(e => new { RegNumber = e.RegistrationNumber, Count = e.Supplies.Count }).ToList();

            // Проверка порядка сортировки (по убыванию количества поставок)
            // E001 имеет 3 поставки (ID: 1, 2, 11)
            Assert.Equal("E001", topEnterprises[0].RegistrationNumber); 
            // E002, E003, E004, E005, E006, E007 имеют по 2 поставки, сортировка по регистрационному номеру
            Assert.Equal("E002", topEnterprises[1].RegistrationNumber); 
            Assert.Equal("E003", topEnterprises[2].RegistrationNumber); 
            Assert.Equal("E004", topEnterprises[3].RegistrationNumber); 
            Assert.Equal("E005", topEnterprises[4].RegistrationNumber); 

            // Проверка количества поставок
            Assert.Equal(3, topEnterprises[0].Supplies.Count);
            Assert.Equal(2, topEnterprises[1].Supplies.Count);
            Assert.Equal(2, topEnterprises[2].Supplies.Count);
            Assert.Equal(2, topEnterprises[3].Supplies.Count);
            Assert.Equal(2, topEnterprises[4].Supplies.Count);
        }

        [Fact]
        public void Test6_GetSuppliersWithMaxSupplyQuantityByPeriod()
        {
            // Arrange
            var startDate = new DateTime(2023, 2, 1);
            var endDate = new DateTime(2023, 2, 28);

            // Act
            var suppliers = _repository.GetSuppliersWithMaxSupplyQuantityByPeriod(startDate, endDate).ToList();

            // Assert
            Assert.Single(suppliers); // Должен быть один поставщик с максимальным количеством
            Assert.Equal("СтройМатериалы", suppliers[0].Name); // СтройМатериалы поставил 10000 единиц кирпича

            // Проверка общего количества поставок за период
            var totalQuantity = suppliers[0].Supplies
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .Sum(s => s.Quantity);

            Assert.Equal(10000, totalQuantity); // Максимальное количество за февраль - 10000 единиц
        }
    }
}