using AutoMapper;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap< IndividualCreateDto, SubsIndividual>();
            CreateMap<SubsCorporate, CorporateCreateDto>().ReverseMap();
            CreateMap<Subscriber, SubscriberCreateDTO>();
        }
    }
}
