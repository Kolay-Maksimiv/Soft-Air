using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SoftAir.Data.Domain.Aircraft;
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
            var aircraftListDto = new List<AircraftListDto>();
            var aircraft = _aircraftRepository.GetAircraftList();

            foreach(var aircraftItem in aircraft)
            {
                var aircraftDto = new AircraftListDto
                {
                    Id = aircraftItem.Id,
                    Name = aircraftItem.Name
                };
                aircraftListDto.Add(aircraftDto);
            }

            return aircraftListDto;
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

        public void AddAircarft(Aircraft aircraft)
        {
            if (aircraft == null)
                throw new ArgumentNullException(nameof(aircraft));
            _aircraftRepository.AddAircarft(aircraft);

        }
        
    }
}
