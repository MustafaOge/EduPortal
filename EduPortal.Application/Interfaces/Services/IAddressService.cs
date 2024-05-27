using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<object>> GetAddressValue();

        Task<String> GetCounter(string doorNumber);
    }
}
