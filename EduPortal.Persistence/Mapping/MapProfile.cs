using AutoMapper;
using EduPortal.Application.DTO_s.Invoice;
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

         
            CreateMap<SubsIndividual, SubsIndividualDto>();

            CreateMap<CreateIndividualDto, SubsIndividual>();
            CreateMap<SubsIndividualDto, SubsIndividual>();


            CreateMap<SubsCorporate, SubsCorporateDto>();

            CreateMap<CreateCorporateDto, SubsCorporate>();
            CreateMap<SubsCorporateDto, SubsCorporate>();

            //CreateMap<CreateIndividualDto, SubsIndividualDto>().ReverseMap();


            CreateMap<SubsCorporate, CreateCorporateDto>().ReverseMap();
            CreateMap<Subscriber, SubscriberCreateDTO>();

            CreateMap<InvoiceComplaintCreateDto, InvoiceComplaint>();



        }
    }
}
