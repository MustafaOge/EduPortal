using EduPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IOutageNotificationRepository : IGenericRepository<OutageNotification, int>
    {
        Task<IEnumerable<string>> GetDistrictsAsync(string province);
        Task<IEnumerable<string>> GetNeighbourhoodsAsync(string district);
        Task<IEnumerable<OutageNotification>> GetOutagesAsync(string ilce, string mahalle, DateTime tarih);

        Task<List<OutageNotification>> GetUnprocessedOutagesAsync();

        void Update(OutageNotification outage);


        IEnumerable<OutageNotification> GetAll();
        IEnumerable<string> GetDistricts();
        IEnumerable<string> GetDistinctDates();
        IEnumerable<OutageNotification> GetOutagesByDateAndDistrict(DateTime selectedDate, string district);

    }
}
