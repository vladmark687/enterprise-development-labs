using DistrictStatisticsManagement.Enums;
using DistrictStatisticsManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DistrictStatisticsManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly DistrictStatisticsRepository _repository;

        public AnalyticsController(DistrictStatisticsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Analytics/enterpriseCountBySupplier
        [HttpGet("enterpriseCountBySupplier")]
        public ActionResult GetEnterpriseCountBySupplier()
        {
            var result = _repository.GetEnterpriseCountBySupplier();
            var formattedResult = result.ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value);
            return Ok(formattedResult);
        }

        // GET: api/Analytics/supplierCountByIndustryAndOwnership
        [HttpGet("supplierCountByIndustryAndOwnership")]
        public ActionResult GetSupplierCountByIndustryAndOwnership()
        {
            var result = _repository.GetSupplierCountByIndustryAndOwnership();
            var formattedResult = result.ToDictionary(
                kvp => $"{kvp.Key.Item1} - {kvp.Key.Item2}",
                kvp => kvp.Value);
            return Ok(formattedResult);
        }

        // GET: api/Analytics/suppliersWithMaxSupplyQuantity
        [HttpGet("suppliersWithMaxSupplyQuantity")]
        public ActionResult GetSuppliersWithMaxSupplyQuantity(DateTime startDate, DateTime endDate)
        {
            var suppliers = _repository.GetSuppliersWithMaxSupplyQuantityByPeriod(startDate, endDate);
            return Ok(suppliers);
        }
    }
}