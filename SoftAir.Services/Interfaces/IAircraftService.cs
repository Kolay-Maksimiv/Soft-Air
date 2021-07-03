using System.Collections.Generic;
using System.Threading.Tasks;
using SoftAir.Data.Dto;

namespace SoftAir.Services.Interfaces
{
    public interface IAircraftService
    {
        Task<List<AircraftDto>> GetAircraftAsync();
        AircraftDto GetAircraftById(int id);
    }
}
