﻿using AutoMapper;
using SoftAir.Data.Dto;
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

        public async Task<List<AircraftDto>> GetAircraftAsync()
        {
            try
            {
                var aircrafts = _aircraftRepository.GetAircraftList();
                var aircraftDto = _mapper.Map<List<AircraftDto>>(aircrafts);

                return aircraftDto;
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
