using DistrictStatisticsManagement.Repository;
using DistrictStatisticsManagement.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DistrictStatisticsManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudOperationsController : ControllerBase
    {
        private readonly DistrictStatisticsRepository _repository;

        public CrudOperationsController(DistrictStatisticsRepository repository)
        {
            _repository = repository;
        }

        // ========== МАССОВОЕ УДАЛЕНИЕ ==========

        /// <summary>
        /// Удалить все предприятия
        /// </summary>
        [HttpDelete("enterprises/all")]
        public ActionResult<object> DeleteAllEnterprises()
        {
            try
            {
                int deletedCount = _repository.RemoveAllEnterprises();
                return Ok(new { message = $"Удалено {deletedCount} предприятий", deletedCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при удалении предприятий", details = ex.Message });
            }
        }

        /// <summary>
        /// Удалить все поставщики
        /// </summary>
        [HttpDelete("suppliers/all")]
        public ActionResult<object> DeleteAllSuppliers()
        {
            try
            {
                int deletedCount = _repository.RemoveAllSuppliers();
                return Ok(new { message = $"Удалено {deletedCount} поставщиков", deletedCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при удалении поставщиков", details = ex.Message });
            }
        }

        /// <summary>
        /// Удалить все поставки
        /// </summary>
        [HttpDelete("supplies/all")]
        public ActionResult<object> DeleteAllSupplies()
        {
            try
            {
                int deletedCount = _repository.RemoveAllSupplies();
                return Ok(new { message = $"Удалено {deletedCount} поставок", deletedCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при удалении поставок", details = ex.Message });
            }
        }

        // ========== УДАЛЕНИЕ ПО УСЛОВИЮ ==========

        /// <summary>
        /// Удалить предприятия по типу отрасли
        /// </summary>
        /// <param name="industryType">Тип отрасли (0-Сельское хозяйство, 1-Транспорт, 2-Легкая промышленность, 3-Тяжелая промышленность, 4-Строительство, 5-Материально-техническое снабжение)</param>
        [HttpDelete("enterprises/by-industry/{industryType}")]
        public ActionResult<object> DeleteEnterprisesByIndustryType(int industryType)
        {
            try
            {
                if (!Enum.IsDefined(typeof(IndustryType), industryType))
                {
                    return BadRequest(new { error = "Неверный тип отрасли. Допустимые значения: 0-5" });
                }

                int deletedCount = _repository.RemoveEnterprisesByIndustryType((IndustryType)industryType);
                return Ok(new { message = $"Удалено {deletedCount} предприятий типа {(IndustryType)industryType}", deletedCount, industryType = (IndustryType)industryType });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при удалении предприятий", details = ex.Message });
            }
        }

        /// <summary>
        /// Удалить поставки старше указанной даты
        /// </summary>
        /// <param name="date">Дата в формате YYYY-MM-DD</param>
        [HttpDelete("supplies/older-than/{date}")]
        public ActionResult<object> DeleteSuppliesOlderThan(DateTime date)
        {
            try
            {
                int deletedCount = _repository.RemoveSuppliesOlderThan(date);
                return Ok(new { message = $"Удалено {deletedCount} поставок старше {date:yyyy-MM-dd}", deletedCount, cutoffDate = date });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при удалении поставок", details = ex.Message });
            }
        }

        // ========== МАССОВОЕ ОБНОВЛЕНИЕ ==========

        /// <summary>
        /// Обновить количество сотрудников предприятий по типу отрасли
        /// </summary>
        /// <param name="industryType">Тип отрасли (0-5)</param>
        /// <param name="additionalEmployees">Количество сотрудников для добавления (может быть отрицательным)</param>
        [HttpPut("enterprises/update-employees")]
        public ActionResult<object> UpdateEmployeeCountByIndustryType([FromQuery] int industryType, [FromQuery] int additionalEmployees)
        {
            try
            {
                if (!Enum.IsDefined(typeof(IndustryType), industryType))
                {
                    return BadRequest(new { error = "Неверный тип отрасли. Допустимые значения: 0-5" });
                }

                int updatedCount = _repository.UpdateEmployeeCountByIndustryType((IndustryType)industryType, additionalEmployees);
                return Ok(new { 
                    message = $"Обновлено {updatedCount} предприятий типа {(IndustryType)industryType}", 
                    updatedCount, 
                    industryType = (IndustryType)industryType, 
                    additionalEmployees 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при обновлении предприятий", details = ex.Message });
            }
        }

        /// <summary>
        /// Обновить стоимость поставок в диапазоне дат (увеличить на процент)
        /// </summary>
        /// <param name="startDate">Начальная дата</param>
        /// <param name="endDate">Конечная дата</param>
        /// <param name="percentageIncrease">Процент увеличения (например, 10 для увеличения на 10%)</param>
        [HttpPut("supplies/update-cost")]
        public ActionResult<object> UpdateSupplyCostByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] decimal percentageIncrease)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest(new { error = "Начальная дата не может быть больше конечной" });
                }

                if (percentageIncrease < -100)
                {
                    return BadRequest(new { error = "Процент увеличения не может быть меньше -100%" });
                }

                int updatedCount = _repository.UpdateSupplyCostByDateRange(startDate, endDate, percentageIncrease);
                return Ok(new { 
                    message = $"Обновлено {updatedCount} поставок в период с {startDate:yyyy-MM-dd} по {endDate:yyyy-MM-dd}", 
                    updatedCount, 
                    startDate, 
                    endDate, 
                    percentageIncrease 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при обновлении поставок", details = ex.Message });
            }
        }

        // ========== ПРИМЕРЫ ДАННЫХ ДЛЯ ТЕСТИРОВАНИЯ ==========

        /// <summary>
        /// Получить примеры данных для создания предприятия
        /// </summary>
        [HttpGet("examples/enterprise")]
        public ActionResult<object> GetEnterpriseExample()
        {
            return Ok(new
            {
                example = new
                {
                    registrationNumber = "REG999",
                    industryType = 0, // Сельское хозяйство
                    name = "Пример Агрофирма",
                    address = "ул. Примерная, 1",
                    phone = "+7-999-123-4567",
                    ownershipType = 3, // Частная
                    employeeCount = 50,
                    totalArea = 1000.50
                },
                industryTypes = new
                {
                    agriculture = 0,
                    transport = 1,
                    lightIndustry = 2,
                    heavyIndustry = 3,
                    construction = 4,
                    materialSupply = 5
                },
                ownershipTypes = new
                {
                    stateFederal = 0,
                    municipalCity = 1,
                    limitedLiability = 2,
                    private_ = 3,
                    jointStock = 4
                }
            });
        }

        /// <summary>
        /// Получить примеры данных для создания поставщика
        /// </summary>
        [HttpGet("examples/supplier")]
        public ActionResult<object> GetSupplierExample()
        {
            return Ok(new
            {
                example = new
                {
                    name = "Пример Поставщик ООО",
                    address = "ул. Поставочная, 15",
                    phone = "+7-999-987-6543"
                }
            });
        }

        /// <summary>
        /// Получить примеры данных для создания поставки
        /// </summary>
        [HttpGet("examples/supply")]
        public ActionResult<object> GetSupplyExample()
        {
            return Ok(new
            {
                example = new
                {
                    enterpriseRegistrationNumber = "REG001", // Должно существовать
                    supplierId = 1, // Должен существовать
                    productName = "Пример товара",
                    quantity = 100,
                    supplyDate = DateTime.Today.ToString("yyyy-MM-dd"),
                    cost = 15000.00
                },
                note = "Убедитесь, что предприятие и поставщик с указанными ID существуют в базе данных"
            });
        }
    }
}