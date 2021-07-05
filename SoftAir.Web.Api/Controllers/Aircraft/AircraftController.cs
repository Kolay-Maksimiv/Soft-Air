using Microsoft.AspNetCore.Mvc;
using SoftAir.Data.Domain.Aircraft;
using SoftAir.Services.Interfaces;
using SoftAir.Web.Api.ViewModels;
using System.Threading.Tasks;

namespace SoftAir.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService) : base()
        {
            _aircraftService = aircraftService;
        }

        [HttpGet("get-aircraft")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAircraftAsync()
        {
            var result = await _aircraftService.GetAllAircraftAsync();

            return Ok(result);
        }

        [HttpPost("add-aircraft")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddAircraftAsync(AircraftViewModel model)
        {
            if(ModelState.IsValid)
            {

            }
            return BadRequest();
        }
    }
}
