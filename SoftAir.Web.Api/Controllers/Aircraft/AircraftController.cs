using Microsoft.AspNetCore.Mvc;
using SoftAir.Data.Domain.Aircraft;
using SoftAir.Services.Interfaces;
using SoftAir.Web.Api.ViewModels;
using System;
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
        public async Task<IActionResult> GetAircraft()
        {
            var result = await _aircraftService.GetAllAircraftAsync();

            return Ok(result);
        }

        [HttpPost("add-aircraft")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddAircraft([FromBody] AircraftViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != null)
                {

                    var aircraft = new Aircraft
                    {
                        Name = model.Name
                    };

                    _aircraftService.AddAircraft(aircraft);

                    return Ok("Aircraft add");
                }
                else
                    return BadRequest();

            }
            return BadRequest();
        }

        [HttpPut("edit-aircraft")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult EditAircraft([FromBody] AircraftViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                var aircraft = new Aircraft
                {
                    Id = model.Id,
                    Name = model.Name
                };

                _aircraftService.UpdateAircraft(aircraft);

                return Ok("Aircraft edit");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var aircraft = await _aircraftService.FindAsync(id);

                if (aircraft == null)
                    return BadRequest();

                _aircraftService.Delete(aircraft);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
