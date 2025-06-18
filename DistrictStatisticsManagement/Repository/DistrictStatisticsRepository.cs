using DistrictStatisticsManagement.Enums;
using DistrictStatisticsManagement.Models;

namespace DistrictStatisticsManagement.Repository
{
    /// <summary>
    /// Репозиторий для работы с данными статистического управления района
    /// </summary>
    public class DistrictStatisticsRepository
    {
        private List<Enterprise> _enterprises = new List<Enterprise>();
        private List<Supplier> _suppliers = new List<Supplier>();
        private List<Supply> _supplies = new List<Supply>();

        /// <summary>
        /// Получить список всех предприятий
        /// </summary>
        public IEnumerable<Enterprise> GetAllEnterprises() => _enterprises;

        /// <summary>
        /// Получить список всех поставщиков
        /// </summary>
        public IEnumerable<Supplier> GetAllSuppliers() => _suppliers;

        /// <summary>
        /// Получить список всех поставок
        /// </summary>
        public IEnumerable<Supply> GetAllSupplies() => _supplies;

        /// <summary>
        /// Добавить предприятие
        /// </summary>
        public void AddEnterprise(Enterprise enterprise)
        {
            if (_enterprises.Any(e => e.RegistrationNumber == enterprise.RegistrationNumber))
            {
                throw new ArgumentException($"Предприятие с регистрационным номером {enterprise.RegistrationNumber} уже существует");
            }

            _enterprises.Add(enterprise);
        }

        /// <summary>
        /// Добавить поставщика
        /// </summary>
        public void AddSupplier(Supplier supplier)
        {
            if (_suppliers.Any(s => s.Id == supplier.Id))
            {
                throw new ArgumentException($"Поставщик с ID {supplier.Id} уже существует");
            }

            _suppliers.Add(supplier);
        }

        /// <summary>
        /// Добавить поставку
        /// </summary>
        public void AddSupply(Supply supply)
        {
            if (_supplies.Any(s => s.Id == supply.Id))
            {
                throw new ArgumentException($"Поставка с ID {supply.Id} уже существует");
            }

            var enterprise = _enterprises.FirstOrDefault(e => e.RegistrationNumber == supply.EnterpriseRegistrationNumber);
            if (enterprise == null)
            {
                throw new ArgumentException($"Предприятие с регистрационным номером {supply.EnterpriseRegistrationNumber} не найдено");
            }

            var supplier = _suppliers.FirstOrDefault(s => s.Id == supply.SupplierId);
            if (supplier == null)
            {
                throw new ArgumentException($"Поставщик с ID {supply.SupplierId} не найден");
            }

            supply.Enterprise = enterprise;
            supply.Supplier = supplier;

            _supplies.Add(supply);
            enterprise.Supplies.Add(supply);
            supplier.Supplies.Add(supply);
        }

        /// <summary>
        /// Получить информацию о конкретном предприятии по регистрационному номеру
        /// </summary>
        public Enterprise? GetEnterpriseByRegistrationNumber(string registrationNumber)
        {
            return _enterprises.FirstOrDefault(e => e.RegistrationNumber == registrationNumber);
        }

        /// <summary>
        /// Получить список поставщиков, поставивших сырье за заданный период, упорядоченный по названию
        /// </summary>
        public IEnumerable<Supplier> GetSuppliersByPeriod(DateTime startDate, DateTime endDate)
        {
            // Получаем все уникальные ID поставщиков за указанный период
            var supplierIds = _supplies
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .Select(s => s.SupplierId)
                .Distinct();

            // Возвращаем поставщиков, отсортированных по имени
            return _suppliers
                .Where(s => supplierIds.Contains(s.Id))
                .OrderBy(s => s.Name);
        }

        /// <summary>
        /// Получить количество предприятий, с которыми работает каждый поставщик
        /// </summary>
        public Dictionary<Supplier, int> GetEnterpriseCountBySupplier()
        {
            var result = new Dictionary<Supplier, int>();
            
            foreach (var supplier in _suppliers)
            {
                // Получаем все уникальные регистрационные номера предприятий для данного поставщика
                var enterpriseCount = _supplies
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
            return _enterprises
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
            var enterprisesWithSupplyCounts = _enterprises
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
            var maxQuantity = _supplies
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .GroupBy(s => s.SupplierId)
                .Select(g => new { SupplierId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
                .Max(x => x.TotalQuantity);

            var supplierIds = _supplies
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .GroupBy(s => s.SupplierId)
                .Where(g => g.Sum(s => s.Quantity) == maxQuantity)
                .Select(g => g.Key);

            return _suppliers.Where(s => supplierIds.Contains(s.Id));
        }

        /// <summary>
        /// Получить количество предприятий для конкретного поставщика по его имени
        /// </summary>
        public int GetEnterpriseCountBySupplier(string supplierName)
        {
            var supplier = _suppliers.FirstOrDefault(s => s.Name == supplierName);
            if (supplier == null)
            {
                return 0;
            }

            // Получаем все уникальные регистрационные номера предприятий для данного поставщика
            return _supplies
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
            _enterprises.Clear();
            _suppliers.Clear();
            _supplies.Clear();
        }

        /// <summary>
        /// Удалить предприятие по регистрационному номеру
        /// </summary>
        public bool RemoveEnterprise(string registrationNumber)
        {
            var enterprise = _enterprises.FirstOrDefault(e => e.RegistrationNumber == registrationNumber);
            if (enterprise == null)
            {
                return false;
            }

            // Удаляем связанные поставки
            var suppliesToRemove = _supplies.Where(s => s.EnterpriseRegistrationNumber == registrationNumber).ToList();
            foreach (var supply in suppliesToRemove)
            {
                var supplier = _suppliers.FirstOrDefault(s => s.Id == supply.SupplierId);
                if (supplier != null)
                {
                    supplier.Supplies.Remove(supply);
                }
                _supplies.Remove(supply);
            }

            return _enterprises.Remove(enterprise);
        }

        /// <summary>
        /// Удалить поставщика по ID
        /// </summary>
        public bool RemoveSupplier(int supplierId)
        {
            var supplier = _suppliers.FirstOrDefault(s => s.Id == supplierId);
            if (supplier == null)
            {
                return false;
            }

            // Удаляем связанные поставки
            var suppliesToRemove = _supplies.Where(s => s.SupplierId == supplierId).ToList();
            foreach (var supply in suppliesToRemove)
            {
                var enterprise = _enterprises.FirstOrDefault(e => e.RegistrationNumber == supply.EnterpriseRegistrationNumber);
                if (enterprise != null)
                {
                    enterprise.Supplies.Remove(supply);
                }
                _supplies.Remove(supply);
            }

            return _suppliers.Remove(supplier);
        }

        /// <summary>
        /// Удалить поставку по ID
        /// </summary>
        public bool RemoveSupply(int supplyId)
        {
            var supply = _supplies.FirstOrDefault(s => s.Id == supplyId);
            if (supply == null)
            {
                return false;
            }

            // Удаляем связи с предприятием и поставщиком
            var enterprise = _enterprises.FirstOrDefault(e => e.RegistrationNumber == supply.EnterpriseRegistrationNumber);
            if (enterprise != null)
            {
                enterprise.Supplies.Remove(supply);
            }

            var supplier = _suppliers.FirstOrDefault(s => s.Id == supply.SupplierId);
            if (supplier != null)
            {
                supplier.Supplies.Remove(supply);
            }

            return _supplies.Remove(supply);
        }
    }
}