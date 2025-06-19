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

        // GET: api/Supply/{id}
        [HttpGet("{id}")]
        public ActionResult<Supply> GetSupply(int id)
        {
            var supply = _repository.GetAllSupplies().FirstOrDefault(s => s.Id == id);

            if (supply == null)
            {
                return NotFound();
            }

            return Ok(supply);
        }

        // GET: api/Supply/{id}/details
        [HttpGet("{id}/details")]
        public ActionResult<object> GetSupplyDetails(int id)
        {
            var supply = _repository.GetAllSupplies().FirstOrDefault(s => s.Id == id);

            if (supply == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                supply.Id,
                supply.EnterpriseRegistrationNumber,
                EnterpriseName = supply.Enterprise?.Name,
                supply.SupplierId,
                SupplierName = supply.Supplier?.Name,
                supply.ProductName,
                supply.Quantity,
                supply.Cost,
                supply.SupplyDate,
                TotalValue = supply.Quantity * supply.Cost
            });
        }

        // PUT: api/Supply/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateSupply(int id, Supply supply)
        {
            if (id != supply.Id)
            {
                return BadRequest("ID в URL не соответствует ID в теле запроса.");
            }

            var existingSupply = _repository.GetSupplyForUpdate(id);
            if (existingSupply == null)
            {
                return NotFound();
            }

            // Обновляем свойства существующей поставки
            existingSupply.EnterpriseRegistrationNumber = supply.EnterpriseRegistrationNumber;
            existingSupply.SupplierId = supply.SupplierId;
            existingSupply.ProductName = supply.ProductName;
            existingSupply.Quantity = supply.Quantity;
            existingSupply.Cost = supply.Cost;
            existingSupply.SupplyDate = supply.SupplyDate;

            _repository.SaveChanges();
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