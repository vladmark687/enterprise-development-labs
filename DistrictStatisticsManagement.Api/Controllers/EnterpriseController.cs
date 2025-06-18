using DistrictStatisticsManagement.Models;
using DistrictStatisticsManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DistrictStatisticsManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseController : ControllerBase
    {
        private readonly DistrictStatisticsRepository _repository;

        public EnterpriseController(DistrictStatisticsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Enterprise
        [HttpGet]
        public ActionResult<IEnumerable<Enterprise>> GetAllEnterprises()
        {
            return Ok(_repository.GetAllEnterprises());
        }

        // GET: api/Enterprise/{registrationNumber}
        [HttpGet("{registrationNumber}")]
        public ActionResult<Enterprise> GetEnterprise(string registrationNumber)
        {
            var enterprise = _repository.GetEnterpriseByRegistrationNumber(registrationNumber);

            if (enterprise == null)
            {
                return NotFound();
            }

            return Ok(enterprise);
        }

        // GET: api/Enterprise/{registrationNumber}/details
        [HttpGet("{registrationNumber}/details")]
        public ActionResult<object> GetEnterpriseDetails(string registrationNumber)
        {
            var enterprise = _repository.GetEnterpriseByRegistrationNumber(registrationNumber);

            if (enterprise == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                enterprise.RegistrationNumber,
                enterprise.Name,
                IndustryType = enterprise.IndustryType.ToString(),
                enterprise.Address,
                enterprise.Phone,
                OwnershipType = enterprise.OwnershipType.ToString(),
                enterprise.EmployeeCount,
                enterprise.TotalArea,
                SuppliesCount = enterprise.Supplies?.Count ?? 0,
                TotalSupplyCost = enterprise.Supplies?.Sum(s => s.Cost) ?? 0
            });
        }

        // POST: api/Enterprise
        [HttpPost]
        public ActionResult<Enterprise> CreateEnterprise(Enterprise enterprise)
        {
            try
            {
                _repository.AddEnterprise(enterprise);
                return CreatedAtAction(nameof(GetEnterprise), new { registrationNumber = enterprise.RegistrationNumber }, enterprise);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Enterprise/top5BySupplyCount
        [HttpGet("top5BySupplyCount")]
        public ActionResult<IEnumerable<Enterprise>> GetTop5EnterprisesBySupplyCount()
        {
            return Ok(_repository.GetTop5EnterprisesBySupplyCount());
        }

        // GET: api/Enterprise/countBySupplier/{supplierName}
        [HttpGet("countBySupplier/{supplierName}")]
        public ActionResult<int> GetEnterpriseCountBySupplier(string supplierName)
        {
            return Ok(_repository.GetEnterpriseCountBySupplier(supplierName));
        }

        // PUT: api/Enterprise/{registrationNumber}
        [HttpPut("{registrationNumber}")]
        public IActionResult UpdateEnterprise(string registrationNumber, Enterprise enterprise)
        {
            if (registrationNumber != enterprise.RegistrationNumber)
            {
                return BadRequest("Registration number in URL does not match the one in the request body.");
            }

            var existingEnterprise = _repository.GetEnterpriseForUpdate(registrationNumber);
            if (existingEnterprise == null)
            {
                return NotFound();
            }

            // Обновляем свойства существующего предприятия
            existingEnterprise.Name = enterprise.Name;
            existingEnterprise.IndustryType = enterprise.IndustryType;
            existingEnterprise.Address = enterprise.Address;
            existingEnterprise.Phone = enterprise.Phone;
            existingEnterprise.OwnershipType = enterprise.OwnershipType;
            existingEnterprise.EmployeeCount = enterprise.EmployeeCount;
            existingEnterprise.TotalArea = enterprise.TotalArea;

            _repository.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Enterprise/{registrationNumber}
        [HttpDelete("{registrationNumber}")]
        public IActionResult DeleteEnterprise(string registrationNumber)
        {
            var enterprise = _repository.GetEnterpriseByRegistrationNumber(registrationNumber);
            if (enterprise == null)
            {
                return NotFound();
            }

            // Удаляем предприятие из репозитория
            bool result = _repository.RemoveEnterprise(registrationNumber);
            if (!result)
            {
                return StatusCode(500, "Не удалось удалить предприятие");
            }
            
            return NoContent();
        }
    }
}