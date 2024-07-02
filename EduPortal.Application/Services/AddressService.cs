using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;

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
