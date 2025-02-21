using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whatsapp.Domain.DTOs;
using whatsapp.Domain.Entities;

namespace whatsapp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<GetDataFromExcelDto, PhoneNumber>();
            CreateMap<PhoneNumber, GetPhoneFromDBDto>();
        }
    }
}
