using DistrictStatisticsManagement.Models;
using DistrictStatisticsManagement.Repository;
using DistrictStatisticsManagement.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DistrictStatisticsManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly DistrictStatisticsRepository _repository;

        public SupplierController(DistrictStatisticsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Supplier
        [HttpGet]
        public ActionResult<IEnumerable<Supplier>> GetAllSuppliers()
        {
            return Ok(_repository.GetAllSuppliers());
        }

        // GET: api/Supplier/byPeriod
        [HttpGet("byPeriod")]
        public ActionResult<IEnumerable<Supplier>> GetSuppliersByPeriod(DateTime startDate, DateTime endDate)
        {
            return Ok(_repository.GetSuppliersByPeriod(startDate, endDate));
        }

        // POST: api/Supplier
        [HttpPost]
        public ActionResult<Supplier> CreateSupplier(Supplier supplier)
        {
            try
            {
                _repository.AddSupplier(supplier);
                return CreatedAtAction(nameof(GetAllSuppliers), null, supplier);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Supplier/countByIndustryAndOwnership
        [HttpGet("countByIndustryAndOwnership")]
        public ActionResult<int> GetSupplierCountByIndustryAndOwnership(int industryType, int ownershipType)
        {
            var result = _repository.GetSupplierCountByIndustryAndOwnership();
            var key = ((IndustryType)industryType, (OwnershipType)ownershipType);
            
            if (result.TryGetValue(key, out int count))
            {
                return Ok(count);
            }
            
            return Ok(0); // Если для указанной комбинации нет данных, возвращаем 0
        }

        // GET: api/Supplier/withMaxSupplyQuantity
        [HttpGet("withMaxSupplyQuantity")]
        public ActionResult<IEnumerable<Supplier>> GetSuppliersWithMaxSupplyQuantity(DateTime startDate, DateTime endDate)
        {
            return Ok(_repository.GetSuppliersWithMaxSupplyQuantityByPeriod(startDate, endDate));
        }

        // PUT: api/Supplier/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest("ID в URL не соответствует ID в теле запроса.");
            }

            var existingSupplier = _repository.GetAllSuppliers().FirstOrDefault(s => s.Id == id);
            if (existingSupplier == null)
            {
                return NotFound();
            }

            // Обновляем свойства существующего поставщика
            existingSupplier.Name = supplier.Name;
            existingSupplier.Address = supplier.Address;
            existingSupplier.Phone = supplier.Phone;

            return NoContent();
        }

        // DELETE: api/Supplier/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _repository.GetAllSuppliers().FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            // Удаляем поставщика из репозитория
            bool result = _repository.RemoveSupplier(id);
            if (!result)
            {
                return StatusCode(500, "Не удалось удалить поставщика");
            }
            
            return NoContent();
        }
    }
}