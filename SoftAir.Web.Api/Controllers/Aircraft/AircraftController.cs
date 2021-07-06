using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SoftAir.Data.Domain.Aircraft;
using SoftAir.Data.Dto.Airctaft;
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
        private IMapper _mapper;

        public AircraftController(IAircraftService aircraftService, IMapper mapper) : base()
        {
            _aircraftService = aircraftService;
            _mapper = mapper;
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
        public IActionResult AddAircraft([FromBody] AircraftViewModel model)
        {
            if (ModelState.IsValid)
            {
                var aircraft = new Aircraft
                {
                    Name = model.Name
                };

                _aircraftService.AddAircarft(aircraft);

                return Ok("Aircraft add");
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
    }
}
