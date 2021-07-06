using Microsoft.AspNetCore.Mvc;
using SoftAir.Data.Domain.Aircraft;
using SoftAir.Data.Dto.Airctaft;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftAir.Services.Interfaces
{
    public interface IAircraftService
    {
        Task<List<AircraftListDto>> GetAllAircraftAsync();
        void AddAircarft(Aircraft aircraft);
    }
}
