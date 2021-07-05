using SoftAir.Data.Dto.Airctaft;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftAir.Services.Interfaces
{
    public interface IAircraftService
    {
        Task<List<AircraftListDto>> GetAllAircraftAsync();
        AircraftDto GetAircraftById(int id);
    }
}
