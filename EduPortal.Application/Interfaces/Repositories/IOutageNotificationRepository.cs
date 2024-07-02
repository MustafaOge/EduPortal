using EduPortal.Domain.Entities;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IOutageNotificationRepository : IGenericRepository<OutageNotification, int>
    {
        /// <summary>
        /// Asynchronously retrieves a list of districts based on the given province.
        /// </summary>
        /// <param name="province">The name of the province.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of district names.</returns>
        Task<IEnumerable<string>> GetDistrictsAsync(string province);

        /// <summary>
        /// Asynchronously retrieves a list of neighborhoods based on the given district.
        /// </summary>
        /// <param name="district">The name of the district.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of neighborhood names.</returns>
        Task<IEnumerable<string>> GetNeighbourhoodsAsync(string district);

        /// <summary>
        /// Asynchronously retrieves a list of outage notifications based on the given district, neighborhood, and date.
        /// </summary>
        /// <param name="ilce">The name of the district.</param>
        /// <param name="mahalle">The name of the neighborhood.</param>
        /// <param name="tarih">The date for the outage notifications.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of outage notifications.</returns>
        Task<IEnumerable<OutageNotification>> GetOutagesAsync(string ilce, string mahalle, DateTime tarih);

        /// <summary>
        /// Asynchronously retrieves a list of unprocessed outage notifications.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a list of unprocessed outage notifications.</returns>
        Task<List<OutageNotification>> GetUnprocessedOutagesAsync();

        /// <summary>
        /// Updates the specified outage notification.
        /// </summary>
        /// <param name="outage">The outage notification to update.</param>
        void Update(OutageNotification outage);

        /// <summary>
        /// Retrieves all outage notifications.
        /// </summary>
        /// <returns>A collection of all outage notifications.</returns>
        IEnumerable<OutageNotification> GetAll();

        /// <summary>
        /// Retrieves a list of all districts.
        /// </summary>
        /// <returns>A collection of district names.</returns>
        IEnumerable<string> GetDistricts();

        /// <summary>
        /// Retrieves a list of distinct dates from the outage notifications.
        /// </summary>
        /// <returns>A collection of distinct dates.</returns>
        IEnumerable<string> GetDistinctDates();

        /// <summary>
        /// Retrieves a list of outage notifications based on the given date and district.
        /// </summary>
        /// <param name="selectedDate">The date for the outage notifications.</param>
        /// <param name="district">The name of the district.</param>
        /// <returns>A collection of outage notifications for the specified date and district.</returns>
        IEnumerable<OutageNotification> GetOutagesByDateAndDistrict(DateTime selectedDate, string district);


    }
}
