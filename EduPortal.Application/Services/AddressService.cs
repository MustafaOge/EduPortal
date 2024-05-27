using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EduPortal.Application.Services
{
    public class AddressService(IAddressRepository addressRepository) : IAddressService
    {
        public async Task<IEnumerable<object>> GetAddressValue()
        {
            IEnumerable<object> address = await addressRepository.GetAddressData();
            return address; 
            
        }

        public async Task<string> GetCounter(string doorNumber)
        {
            var counter = await addressRepository.GetCounterNumber(doorNumber);
            return counter.counterNumber.ToString();
        }

    }
}
