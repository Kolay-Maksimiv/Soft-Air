using SoftAir.Data.Domain.Aircraft;
using System;
using System.Collections.Generic;

namespace SoftAir.Data.Repositories.Interfaces
{
    public interface IAircraftRepository : IDisposable
    {
        IEnumerable<Aircraft> GetAircraftList();
        Aircraft GetAircraft(int id);
    }
}
