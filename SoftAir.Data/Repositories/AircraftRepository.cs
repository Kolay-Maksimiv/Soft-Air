using SoftAir.Data.Domain.Aircraft;
using SoftAir.Data.Repositories.Abstract;
using SoftAir.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SoftAir.Data.Repositories
{
    public class AircraftRepository : GenericRepository<Aircraft>, IAircraftRepository
    {
        public new ApplicationDbContext Context { get; }

        public AircraftRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public IEnumerable<Aircraft> GetAircraftList()
        {
            return Context.Aircraft.ToList();
        }

        public Aircraft GetAircraft(int id)
        {
            return Context.Aircraft.FirstOrDefault(x => x.Id == id);

        }
    }
}
