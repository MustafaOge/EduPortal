using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Core.Responses;

namespace EduPortal.Application.Interfaces.Services
{
    public interface ISubsCorporateService
    {
        Task<Response<SubsCorporateDto>> CreateCorporateAsync(CreateCorporateDto individualCreate);
        Task<SubsCorporateDto> FindCorporateAsync(string TaxIdNumber);
        Task<Response<bool>> TerminateSubsCorporateAsync(string TaxIdNumber);
    }
}


