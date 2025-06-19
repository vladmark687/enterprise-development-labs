using DistrictStatisticsManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistrictStatisticsManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabQueriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LabQueriesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Запрос 1: Вывести все сведения о конкретном предприятии
        /// </summary>
        [HttpGet("enterprise/{registrationNumber}")]
        public async Task<ActionResult> GetEnterpriseDetails(string registrationNumber)
        {
            var enterprise = await _context.Enterprises
                .Where(e => e.RegistrationNumber == registrationNumber)
                .Select(e => new
                {
                    РегистрационныйНомер = e.RegistrationNumber,
                    Наименование = e.Name,
                    Адрес = e.Address,
                    Телефон = e.Phone,
                    ТипОтрасли = e.IndustryType.ToString(),
                    ФормаСобственности = e.OwnershipType.ToString(),
                    КоличествоРаботающих = e.EmployeeCount,
                    ОбщаяПлощадь = e.TotalArea
                })
                .FirstOrDefaultAsync();

            if (enterprise == null)
            {
                return NotFound($"Предприятие с регистрационным номером {registrationNumber} не найдено");
            }

            return Ok(enterprise);
        }

        /// <summary>
        /// Запрос 2: Вывести всех поставщиков, поставивших сырье за заданный период, упорядочить по названию
        /// </summary>
        [HttpGet("suppliers-by-period")]
        public async Task<ActionResult> GetSuppliersByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.Supplies.Any(sup => sup.SupplyDate >= startDate && sup.SupplyDate <= endDate))
                .Select(s => new
                {
                    IDПоставщика = s.Id,
                    НаименованиеПоставщика = s.Name,
                    Адрес = s.Address,
                    Телефон = s.Phone
                })
                .OrderBy(s => s.НаименованиеПоставщика)
                .ToListAsync();

            return Ok(suppliers);
        }

        /// <summary>
        /// Запрос 3: Вывести количество предприятий, с которыми работает каждый поставщик
        /// </summary>
        [HttpGet("enterprise-count-by-supplier")]
        public async Task<ActionResult> GetEnterpriseCountBySupplier()
        {
            var result = await _context.Suppliers
                .Select(s => new
                {
                    IDПоставщика = s.Id,
                    НаименованиеПоставщика = s.Name,
                    КоличествоПредприятий = s.Supplies
                        .Select(sup => sup.EnterpriseRegistrationNumber)
                        .Distinct()
                        .Count()
                })
                .OrderByDescending(x => x.КоличествоПредприятий)
                .ThenBy(x => x.НаименованиеПоставщика)
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Запрос 4: Вывести информацию о количестве поставщиков для каждого типа отрасли и форме собственности
        /// </summary>
        [HttpGet("supplier-count-by-industry-ownership")]
        public async Task<ActionResult> GetSupplierCountByIndustryAndOwnership()
        {
            var result = await _context.Enterprises
                .GroupBy(e => new { e.IndustryType, e.OwnershipType })
                .Select(g => new
                {
                    ТипОтрасли = g.Key.IndustryType.ToString(),
                    ФормаСобственности = g.Key.OwnershipType.ToString(),
                    КоличествоПоставщиков = g.SelectMany(e => e.Supplies)
                        .Select(s => s.SupplierId)
                        .Distinct()
                        .Count()
                })
                .OrderBy(x => x.ТипОтрасли)
                .ThenBy(x => x.ФормаСобственности)
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Запрос 5: Вывести топ 5 предприятий по количеству поставок
        /// </summary>
        [HttpGet("top-enterprises-by-supply-count")]
        public async Task<ActionResult> GetTopEnterprisesBySupplyCount()
        {
            var result = await _context.Enterprises
                .Select(e => new
                {
                    РегистрационныйНомер = e.RegistrationNumber,
                    НаименованиеПредприятия = e.Name,
                    КоличествоПоставок = e.Supplies.Count()
                })
                .OrderByDescending(x => x.КоличествоПоставок)
                .Take(5)
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Запрос 6: Вывести информацию о поставщиках, поставивших максимальное количество товара за указанный период
        /// </summary>
        [HttpGet("suppliers-max-quantity")]
        public async Task<ActionResult> GetSuppliersWithMaxQuantity([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Сначала получаем общее количество для каждого поставщика
            var supplierTotals = await _context.Suppliers
                .Where(s => s.Supplies.Any(sup => sup.SupplyDate >= startDate && sup.SupplyDate <= endDate))
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Phone = s.Phone,
                    TotalQuantity = s.Supplies
                        .Where(sup => sup.SupplyDate >= startDate && sup.SupplyDate <= endDate)
                        .Sum(sup => sup.Quantity)
                })
                .ToListAsync();

            if (!supplierTotals.Any())
            {
                return Ok(new List<object>());
            }

            // Находим максимальное количество
            var maxQuantity = supplierTotals.Max(st => st.TotalQuantity);

            // Возвращаем поставщиков с максимальным количеством
            var result = supplierTotals
                .Where(st => st.TotalQuantity == maxQuantity)
                .Select(st => new
                {
                    IDПоставщика = st.Id,
                    НаименованиеПоставщика = st.Name,
                    Адрес = st.Address,
                    Телефон = st.Phone,
                    ОбщееКоличествоТовара = st.TotalQuantity
                })
                .OrderBy(x => x.НаименованиеПоставщика)
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Дополнительный запрос: Получить статистику по всем предприятиям
        /// </summary>
        [HttpGet("enterprise-statistics")]
        public async Task<ActionResult> GetEnterpriseStatistics()
        {
            var statistics = new
            {
                ОбщееКоличествоПредприятий = await _context.Enterprises.CountAsync(),
                ОбщееКоличествоПоставщиков = await _context.Suppliers.CountAsync(),
                ОбщееКоличествоПоставок = await _context.Supplies.CountAsync(),
                СтатистикаПоОтраслям = await _context.Enterprises
                    .GroupBy(e => e.IndustryType)
                    .Select(g => new
                    {
                        Отрасль = g.Key.ToString(),
                        КоличествоПредприятий = g.Count(),
                        ОбщееКоличествоСотрудников = g.Sum(e => e.EmployeeCount),
                        ОбщаяПлощадь = g.Sum(e => e.TotalArea)
                    })
                    .ToListAsync(),
                СтатистикаПоФормамСобственности = await _context.Enterprises
                    .GroupBy(e => e.OwnershipType)
                    .Select(g => new
                    {
                        ФормаСобственности = g.Key.ToString(),
                        КоличествоПредприятий = g.Count(),
                        ОбщееКоличествоСотрудников = g.Sum(e => e.EmployeeCount)
                    })
                    .ToListAsync()
            };

            return Ok(statistics);
        }
    }
}