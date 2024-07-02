using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Core.Responses;

namespace EduPortal.Application.Interfaces.Services
{
    public interface ISubsIndividualService
    {
        Task<Response<SubsIndividualDto>> CreateIndividualAsync(CreateIndividualDto individualCreate);
        Task<SubsIndividualDto> FindIndividualDtoAsync(string IdentityNumber);
        Task<Response<bool>> TerminateSubsIndividualAsync(string identityNumber);
    }
}

