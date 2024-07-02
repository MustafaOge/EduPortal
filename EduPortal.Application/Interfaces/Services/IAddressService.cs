namespace EduPortal.Application.Interfaces.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<object>> GetAddressValue();

        Task<String> GetCounter(string doorNumber);
    }
}
