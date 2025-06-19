using DistrictStatisticsManagement.Data;
using DistrictStatisticsManagement.Enums;
using DistrictStatisticsManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DistrictStatisticsManagement.Repository
{
    /// <summary>
    /// Репозиторий для работы с данными статистического управления района
    /// </summary>
    public class DistrictStatisticsRepository
    {
        private readonly AppDbContext _context;

        public DistrictStatisticsRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список всех предприятий
        /// </summary>
        public IEnumerable<Enterprise> GetAllEnterprises() => _context.Enterprises.Include(e => e.Supplies).ToList();

        /// <summary>
        /// Получить список всех поставщиков
        /// </summary>
        public IEnumerable<Supplier> GetAllSuppliers() => _context.Suppliers.Include(s => s.Supplies).ToList();

        /// <summary>
        /// Получить список всех поставок
        /// </summary>
        public IEnumerable<Supply> GetAllSupplies() => _context.Supplies.Include(s => s.Enterprise).Include(s => s.Supplier).ToList();

        /// <summary>
        /// Добавить предприятие
        /// </summary>
        public void AddEnterprise(Enterprise enterprise)
        {
            if (_context.Enterprises.Any(e => e.RegistrationNumber == enterprise.RegistrationNumber))
            {
                throw new ArgumentException($"Предприятие с регистрационным номером {enterprise.RegistrationNumber} уже существует");
            }

            _context.Enterprises.Add(enterprise);
            _context.SaveChanges();
        }

        /// <summary>
        /// Добавить поставщика
        /// </summary>
        public void AddSupplier(Supplier supplier)
        {
            if (_context.Suppliers.Any(s => s.Id == supplier.Id && supplier.Id != 0))
            {
                throw new ArgumentException($"Поставщик с ID {supplier.Id} уже существует");
            }

            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
        }

        /// <summary>
        /// Добавить поставку
        /// </summary>
        public void AddSupply(Supply supply)
        {
            if (_context.Supplies.Any(s => s.Id == supply.Id && supply.Id != 0))
            {
                throw new ArgumentException($"Поставка с ID {supply.Id} уже существует");
            }

            var enterprise = _context.Enterprises.FirstOrDefault(e => e.RegistrationNumber == supply.EnterpriseRegistrationNumber);
            if (enterprise == null)
            {
                throw new ArgumentException($"Предприятие с регистрационным номером {supply.EnterpriseRegistrationNumber} не найдено");
            }

            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == supply.SupplierId);
            if (supplier == null)
            {
                throw new ArgumentException($"Поставщик с ID {supply.SupplierId} не найден");
            }

            _context.Supplies.Add(supply);
            _context.SaveChanges();
        }

        /// <summary>
        /// Получить информацию о конкретном предприятии по регистрационному номеру
        /// </summary>
        public Enterprise? GetEnterpriseByRegistrationNumber(string registrationNumber)
        {
            return _context.Enterprises
                .Include(e => e.Supplies)
                .ThenInclude(s => s.Supplier)
                .FirstOrDefault(e => e.RegistrationNumber == registrationNumber);
        }

        /// <summary>
        /// Получить список поставщиков, поставивших сырье за заданный период, упорядоченный по названию
        /// </summary>
        public IEnumerable<Supplier> GetSuppliersByPeriod(DateTime startDate, DateTime endDate)
        {
            return _context.Suppliers
                .Where(s => s.Supplies.Any(supply => supply.SupplyDate >= startDate && supply.SupplyDate <= endDate))
                .OrderBy(s => s.Name)
                .ToList();
        }

        /// <summary>
        /// Получить количество предприятий, с которыми работает каждый поставщик
        /// </summary>
        public Dictionary<Supplier, int> GetEnterpriseCountBySupplier()
        {
            var result = new Dictionary<Supplier, int>();
            
            var suppliers = _context.Suppliers.Include(s => s.Supplies).ToList();
            
            foreach (var supplier in suppliers)
            {
                // Получаем все уникальные регистрационные номера предприятий для данного поставщика
                var enterpriseCount = _context.Supplies
                    .Where(s => s.SupplierId == supplier.Id)
                    .Select(s => s.EnterpriseRegistrationNumber)
                    .Distinct()
                    .Count();
                
                result.Add(supplier, enterpriseCount);
            }
            
            return result;
        }

        /// <summary>
        /// Получить информацию о количестве поставщиков для каждого типа отрасли и форме собственности
        /// </summary>
        public Dictionary<(IndustryType, OwnershipType), int> GetSupplierCountByIndustryAndOwnership()
        {
            // Загружаем предприятия с их поставками
            var enterprises = _context.Enterprises
                .Include(e => e.Supplies)
                .ToList();
                
            // Выполняем группировку в памяти
            return enterprises
                .GroupBy(e => (e.IndustryType, e.OwnershipType))
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .SelectMany(e => e.Supplies)
                        .Select(s => s.SupplierId)
                        .Distinct()
                        .Count());
        }

        /// <summary>
        /// Получить топ 5 предприятий по количеству поставок
        /// </summary>
        public IEnumerable<Enterprise> GetTop5EnterprisesBySupplyCount()
        {
            // Создаем список предприятий с их количеством поставок для отладки
            var enterprisesWithSupplyCounts = _context.Enterprises
                .Include(e => e.Supplies)
                .Select(e => new { Enterprise = e, SupplyCount = e.Supplies.Count })
                .OrderByDescending(x => x.SupplyCount)
                .ThenBy(x => x.Enterprise.RegistrationNumber)
                .ToList();

            // Возвращаем топ 5 предприятий
            return enterprisesWithSupplyCounts
                .Take(5)
                .Select(x => x.Enterprise);
        }

        /// <summary>
        /// Получить информацию о поставщиках, поставивших максимальное количество товара за указанный период
        /// </summary>
        public IEnumerable<Supplier> GetSuppliersWithMaxSupplyQuantityByPeriod(DateTime startDate, DateTime endDate)
        {
            var maxQuantity = _context.Supplies
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .GroupBy(s => s.SupplierId)
                .Select(g => new { SupplierId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
                .Max(x => x.TotalQuantity);

            var supplierIds = _context.Supplies
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .GroupBy(s => s.SupplierId)
                .Where(g => g.Sum(s => s.Quantity) == maxQuantity)
                .Select(g => g.Key);

            return _context.Suppliers.Where(s => supplierIds.Contains(s.Id)).ToList();
        }

        /// <summary>
        /// Получить количество предприятий для конкретного поставщика по его имени
        /// </summary>
        public int GetEnterpriseCountBySupplier(string supplierName)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Name == supplierName);
            if (supplier == null)
            {
                return 0;
            }

            // Получаем все уникальные регистрационные номера предприятий для данного поставщика
            return _context.Supplies
                .Where(s => s.SupplierId == supplier.Id)
                .Select(s => s.EnterpriseRegistrationNumber)
                .Distinct()
                .Count();
        }

        /// <summary>
        /// Очистить все данные в репозитории
        /// </summary>
        public void ClearData()
        {
            _context.Supplies.RemoveRange(_context.Supplies);
            _context.Suppliers.RemoveRange(_context.Suppliers);
            _context.Enterprises.RemoveRange(_context.Enterprises);
            _context.SaveChanges();
        }

        /// <summary>
        /// Удалить предприятие по регистрационному номеру
        /// </summary>
        public bool RemoveEnterprise(string registrationNumber)
        {
            var enterprise = _context.Enterprises
                .Include(e => e.Supplies)
                .FirstOrDefault(e => e.RegistrationNumber == registrationNumber);
                
            if (enterprise == null)
            {
                return false;
            }

            // Удаляем связанные поставки
            var suppliesToRemove = _context.Supplies
                .Where(s => s.EnterpriseRegistrationNumber == registrationNumber)
                .ToList();
                
            _context.Supplies.RemoveRange(suppliesToRemove);
            _context.Enterprises.Remove(enterprise);
            _context.SaveChanges();
            
            return true;
        }

        /// <summary>
        /// Удалить поставщика по ID
        /// </summary>
        public bool RemoveSupplier(int supplierId)
        {
            var supplier = _context.Suppliers
                .Include(s => s.Supplies)
                .FirstOrDefault(s => s.Id == supplierId);
                
            if (supplier == null)
            {
                return false;
            }

            // Удаляем связанные поставки
            var suppliesToRemove = _context.Supplies
                .Where(s => s.SupplierId == supplierId)
                .ToList();
                
            _context.Supplies.RemoveRange(suppliesToRemove);
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            
            return true;
        }

        /// <summary>
        /// Удалить поставку по ID
        /// </summary>
        public bool RemoveSupply(int supplyId)
        {
            var supply = _context.Supplies.FirstOrDefault(s => s.Id == supplyId);
            if (supply == null)
            {
                return false;
            }

            _context.Supplies.Remove(supply);
            _context.SaveChanges();
            
            return true;
        }

        /// <summary>
        /// Удалить все предприятия
        /// </summary>
        public int RemoveAllEnterprises()
        {
            var enterprises = _context.Enterprises.ToList();
            var count = enterprises.Count;
            
            // Сначала удаляем все поставки
            var supplies = _context.Supplies.ToList();
            _context.Supplies.RemoveRange(supplies);
            
            // Затем удаляем предприятия
            _context.Enterprises.RemoveRange(enterprises);
            _context.SaveChanges();
            
            return count;
        }

        /// <summary>
        /// Удалить все поставщики
        /// </summary>
        public int RemoveAllSuppliers()
        {
            var suppliers = _context.Suppliers.ToList();
            var count = suppliers.Count;
            
            // Сначала удаляем все поставки
            var supplies = _context.Supplies.ToList();
            _context.Supplies.RemoveRange(supplies);
            
            // Затем удаляем поставщиков
            _context.Suppliers.RemoveRange(suppliers);
            _context.SaveChanges();
            
            return count;
        }

        /// <summary>
        /// Удалить все поставки
        /// </summary>
        public int RemoveAllSupplies()
        {
            var supplies = _context.Supplies.ToList();
            var count = supplies.Count;
            
            _context.Supplies.RemoveRange(supplies);
            _context.SaveChanges();
            
            return count;
        }

        /// <summary>
        /// Удалить предприятия по условию (по типу отрасли)
        /// </summary>
        public int RemoveEnterprisesByIndustryType(IndustryType industryType)
        {
            var enterprises = _context.Enterprises
                .Where(e => e.IndustryType == industryType)
                .ToList();
            var count = enterprises.Count;
            
            if (count > 0)
            {
                // Удаляем связанные поставки
                var registrationNumbers = enterprises.Select(e => e.RegistrationNumber).ToList();
                var supplies = _context.Supplies
                    .Where(s => registrationNumbers.Contains(s.EnterpriseRegistrationNumber))
                    .ToList();
                
                _context.Supplies.RemoveRange(supplies);
                _context.Enterprises.RemoveRange(enterprises);
                _context.SaveChanges();
            }
            
            return count;
        }

        /// <summary>
        /// Удалить поставки по дате (старше указанной даты)
        /// </summary>
        public int RemoveSuppliesOlderThan(DateTime date)
        {
            var supplies = _context.Supplies
                .Where(s => s.SupplyDate < date)
                .ToList();
            var count = supplies.Count;
            
            if (count > 0)
            {
                _context.Supplies.RemoveRange(supplies);
                _context.SaveChanges();
            }
            
            return count;
        }

        /// <summary>
        /// Обновить количество сотрудников предприятий по типу отрасли
        /// </summary>
        public int UpdateEmployeeCountByIndustryType(IndustryType industryType, int additionalEmployees)
        {
            var enterprises = _context.Enterprises
                .Where(e => e.IndustryType == industryType)
                .ToList();
            
            foreach (var enterprise in enterprises)
            {
                enterprise.EmployeeCount += additionalEmployees;
            }
            
            _context.SaveChanges();
            return enterprises.Count;
        }

        /// <summary>
        /// Обновить стоимость поставок в указанном диапазоне дат (увеличить на процент)
        /// </summary>
        public int UpdateSupplyCostByDateRange(DateTime startDate, DateTime endDate, decimal percentageIncrease)
        {
            var supplies = _context.Supplies
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .ToList();
            
            foreach (var supply in supplies)
            {
                supply.Cost *= (1 + percentageIncrease / 100);
            }
            
            _context.SaveChanges();
            return supplies.Count;
        }

        /// <summary>
        /// Получить предприятие по ID для обновления
        /// </summary>
        public Enterprise? GetEnterpriseForUpdate(string registrationNumber)
        {
            return _context.Enterprises.FirstOrDefault(e => e.RegistrationNumber == registrationNumber);
        }

        /// <summary>
        /// Получить поставщика по ID для обновления
        /// </summary>
        public Supplier? GetSupplierForUpdate(int id)
        {
            return _context.Suppliers.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Получить поставку по ID для обновления
        /// </summary>
        public Supply? GetSupplyForUpdate(int id)
        {
            return _context.Supplies.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Сохранить изменения в контексте
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}