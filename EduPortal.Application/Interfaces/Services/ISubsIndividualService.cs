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
    public interface ISubsIndividualService
    {
        Task<Response<SubsIndividualDto>> CreateIndividualAsync(CreateIndividualDto individualCreate);
        Task<SubsIndividualDto> FindIndividualDtoAsync(string IdentityNumber);
        Task<Response<bool>> TerminateSubsIndividualAsync(string identityNumber);
    }
}

