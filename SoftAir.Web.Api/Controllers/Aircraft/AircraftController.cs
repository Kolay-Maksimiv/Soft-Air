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

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAircraftAsync()
        {
            var result = await _aircraftService.GetAllAircraftAsync();

            return Ok(result);
        }
    }
}
