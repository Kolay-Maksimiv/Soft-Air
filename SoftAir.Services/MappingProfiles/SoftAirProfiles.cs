﻿using AutoMapper;
using SoftAir.Data.Domain.Aircraft;
using SoftAir.Data.Dto.Airctaft;

namespace SoftAir.Services.MappingProfiles
{
    public class SoftAirProfiles : Profile
    {
        public SoftAirProfiles()
        {
            CreateMap<Aircraft, AircraftDto>();
            CreateMap<AircraftDto, Aircraft>();

        }
    }
}
