using EduPortal.Domain.Entities;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IAddressRepository:  IGenericRepository<Ad_IcKapi, int>
    {
        /// <summary>
        /// Asynchronously creates counter numbers for internal doors and adds them to the database.
        /// If a counter number already exists for an internal door, it updates the existing one.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateCounterNumberAsync();

        /// <summary>
        /// Asynchronously retrieves address-related data from the database.
        /// </summary>
        /// <returns>A collection of objects containing address-related data, including districts, neighborhoods, streets, external doors, and internal doors.</returns>
        Task<IEnumerable<object>> GetAddressData();

        /// <summary>
        /// Asynchronously retrieves address-counter data from the database.
        /// </summary>
        /// <param name="doorNumber"></param>
        /// <returns></returns>
        Task<Ad_Sayac> GetCounterNumber(string doorNumber);

        /// <summary>
        /// Asynchronously retrieves the unique identifier (ID) of a neighborhood based on its name.
        /// </summary>
        /// <param name="mahalleAdi">The name of the neighborhood.</param>
        /// <returns>A task that represents the asynchronous operation, containing the ID of the neighborhood.</returns>
        Task<int> GetMahalleKimlikNoByNameAsync(string mahalleAdi);

        /// <summary>
        /// Retrieves a list of subscribers based on a list of neighborhood IDs.
        /// </summary>
        /// <param name="mahalleKimlikNumarasi">A list of neighborhood IDs.</param>
        /// <returns>A list of subscribers.</returns>
        List<Subscriber> GetSubscriberList(List<long> mahalleKimlikNumarasi);

        /// <summary>
        /// Asynchronously retrieves information about an internal door for a given neighborhood ID.
        /// </summary>
        /// <param name="neighborhoodId">The ID of the neighborhood.</param>
        /// <returns>A task that represents the asynchronous operation, containing the information about the internal door associated with the neighborhood ID.</returns>
        Task<Ad_IcKapi> GetAsync(int neighborhoodId);



    }

}
