using SoftAir.Data.Domain.Aircraft;
using SoftAir.Data.Repositories.Interfaces;
using SoftAir.Services.Interfaces;

namespace SoftAir.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly IRepository<Aircraft> _aircraftRepository;
        public AircraftService(IRepository<Aircraft> aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }
    }
}
