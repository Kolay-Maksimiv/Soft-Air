using System.Collections.Generic;
using SoftAir.Data.Dto;

namespace SoftAir.Services.Interfaces
{
    public interface IAircraftService
    {
        List<AircraftDto> Get();
        AircraftDto GetById(int id);
    }
}
