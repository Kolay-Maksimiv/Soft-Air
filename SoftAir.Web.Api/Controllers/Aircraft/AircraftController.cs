using Microsoft.AspNetCore.Mvc;
using SoftAir.Services.Interfaces;
using System.Threading.Tasks;

namespace SoftAir.Web.Api.Controllers.Aircraft
{
    [Route("api/[controller]")]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService) : base()
        {
            _aircraftService = aircraftService;
        }

        /// <summary>
        /// The get API items based on provided JSON object.
        /// </summary>
        /// <returns>The response is 200 (Status - Ok).</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCompanyAsync()
        {
            var result = await _aircraftService.GetAircraftAsync();

            return Ok(result);
        }
    }
}
