using Microsoft.AspNetCore.Mvc;
using SoftAir.Data.Domain.Aircraft;
using SoftAir.Data.Dto.Airctaft;
using SoftAir.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftAir.Services.Interfaces
{
    public interface IAircraftService : IGeneralService<Aircraft>
    {
        Task<List<AircraftListDto>> GetAllAircraftAsync();
        void AddAircraft(Aircraft aircraft);
        void UpdateAircraft(Aircraft aircraft);
    }
}
