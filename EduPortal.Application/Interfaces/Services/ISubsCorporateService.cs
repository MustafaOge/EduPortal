using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface ISubsCorporateService
    {
        Task<Response<SubsCorporateDto>> CreateCorporateAsync(CreateCorporateDto individualCreate);

        Task<Response<List<SubsCorporateDto>>> FindCorporateAsync(string TaxIdNumber);




        //Task<Response<List<SubsCorporateDto>>> FindCorporate(string TaxIdNumber);

        //Response<List<SubsIndividual>> GetAll();
        //Response<SubsIndividual?> GetById(int id);
        //Response<SubscriberResponseResponseDTO> Save(SubscriberCreateDTO subscriber);
        //Response<string> Update(SubsIndividual subscriber);
        //Response<string> DeleteById(int id);

    }
}


