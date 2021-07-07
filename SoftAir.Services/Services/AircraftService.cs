using Microsoft.EntityFrameworkCore;
using SoftAir.Data.Abstract;
using SoftAir.Data.Domain.Aircraft;
using SoftAir.Data.Dto.Airctaft;
using SoftAir.Services.Abstract;
using SoftAir.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftAir.Services.Services
{
    public class AircraftService : GeneralService<Aircraft>, IAircraftService
    {
        public AircraftService(IGenericRepository<Aircraft> repository) : base(repository) { }

        public async Task<List<AircraftListDto>> GetAllAircraftAsync()
        {
            var aircraftListDto = new List<AircraftListDto>();
            var aircraft = await Repository.Table.ToListAsync();

            foreach (var item in aircraft)
            {
                var aircraftDto = new AircraftListDto
                {
                    Id = item.Id,
                    Name = item.Name
                };
                aircraftListDto.Add(aircraftDto);
            }

            return aircraftListDto;
        }

        public void AddAircraft(Aircraft aircraft)
        {
            if (aircraft == null)
                throw new ArgumentNullException(nameof(aircraft));

            Repository.Insert(aircraft);
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            if (aircraft == null)
                throw new ArgumentNullException(nameof(aircraft));

            Repository.Update(aircraft);
        }
    }
}
