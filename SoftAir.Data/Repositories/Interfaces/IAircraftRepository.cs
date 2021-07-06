using SoftAir.Data.Domain.Aircraft;
using System;
using System.Collections.Generic;

namespace SoftAir.Data.Repositories.Interfaces
{
    public interface IAircraftRepository : IDisposable
    {
        IEnumerable<Aircraft> GetAircraftList();
        Aircraft GetAircraft(int id);
        void AddAircarft(Aircraft aircraft);
        void EditAircraft(Aircraft aircraft);
        void DeleteAircraft(int id);

    }
}
