using AutoMapper;
using SoftAir.Data.Dto.Airctaft;
using SoftAir.Data.Repositories.Interfaces;
using SoftAir.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftAir.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private IMapper _mapper;

        public AircraftService(IAircraftRepository aircraftRepository, IMapper mapper)
        {
            _aircraftRepository = aircraftRepository;
            _mapper = mapper;
        }

        public async Task<List<AircraftListDto>> GetAllAircraftAsync()
        {
            try
            {
                var aircrafts = _aircraftRepository.GetAircraftList();
                var aircraftListDto = _mapper.Map<List<AircraftListDto>>(aircrafts);

                return aircraftListDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AircraftDto GetAircraftById(int id)
        {
            try
            {
                var aircraft = _aircraftRepository.GetAircraft(id);
                var aircraftDto = _mapper.Map<AircraftDto>(aircraft);

                return aircraftDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
