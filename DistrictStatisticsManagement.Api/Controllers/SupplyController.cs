using DistrictStatisticsManagement.Models;
using DistrictStatisticsManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DistrictStatisticsManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController : ControllerBase
    {
        private readonly DistrictStatisticsRepository _repository;

        public SupplyController(DistrictStatisticsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Supply
        [HttpGet]
        public ActionResult<IEnumerable<Supply>> GetAllSupplies()
        {
            return Ok(_repository.GetAllSupplies());
        }

        // POST: api/Supply
        [HttpPost]
        public ActionResult<Supply> CreateSupply(Supply supply)
        {
            try
            {
                _repository.AddSupply(supply);
                return CreatedAtAction(nameof(GetAllSupplies), null, supply);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Supply/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateSupply(int id, Supply supply)
        {
            if (id != supply.Id)
            {
                return BadRequest("ID в URL не соответствует ID в теле запроса.");
            }

            var existingSupply = _repository.GetAllSupplies().FirstOrDefault(s => s.Id == id);
            if (existingSupply == null)
            {
                return NotFound();
            }

            // Проверяем существование предприятия и поставщика
            var enterprise = _repository.GetEnterpriseByRegistrationNumber(supply.EnterpriseRegistrationNumber);
            if (enterprise == null)
            {
                return BadRequest($"Предприятие с регистрационным номером {supply.EnterpriseRegistrationNumber} не найдено.");
            }

            var supplier = _repository.GetAllSuppliers().FirstOrDefault(s => s.Id == supply.SupplierId);
            if (supplier == null)
            {
                return BadRequest($"Поставщик с ID {supply.SupplierId} не найден.");
            }

            // Обновляем свойства существующей поставки
            existingSupply.EnterpriseRegistrationNumber = supply.EnterpriseRegistrationNumber;
            existingSupply.SupplierId = supply.SupplierId;
            existingSupply.ProductName = supply.ProductName;
            existingSupply.Quantity = supply.Quantity;
            existingSupply.SupplyDate = supply.SupplyDate;

            return NoContent();
        }

        // DELETE: api/Supply/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSupply(int id)
        {
            var supply = _repository.GetAllSupplies().FirstOrDefault(s => s.Id == id);
            if (supply == null)
            {
                return NotFound();
            }

            // Удаляем поставку из репозитория
            bool result = _repository.RemoveSupply(id);
            if (!result)
            {
                return StatusCode(500, "Не удалось удалить поставку");
            }
            
            return NoContent();
        }
    }
}