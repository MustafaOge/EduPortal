using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;

namespace EduPortal.Persistence.Repositories
{
    public class OutageNotificationRepository : GenericRepository<OutageNotification, int>, IOutageNotificationRepository
    {
        public AppDbContext _context;
        private readonly IGenericRepository<OutageNotification, int> _genericRepository;
        public OutageNotificationRepository(AppDbContext context, IGenericRepository<OutageNotification, int> genericRepository) : base(context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        public void Update(OutageNotification outage)
        {
            _context.OutageNotification.Update(outage);
            _context.SaveChanges();
        }
        public async Task<IEnumerable<string>> GetDistrictsAsync(string province)
        {
            return await _context.OutageNotification
                .Where(o => o.Province == province)
                .Select(o => o.District)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetNeighbourhoodsAsync(string district)
        {
            var neighbourhoods = await _context.OutageNotification
                .Where(o => o.District == district)
                .Select(o => o.EffectedNeighbourhoods)
                .ToListAsync();

            return neighbourhoods.SelectMany(n => n.Split('|')).Distinct();
        }

        public async Task<IEnumerable<OutageNotification>> GetOutagesAsync(string ilce, string mahalle, DateTime tarih)
        {
            // Tarih ile sorgu yaparak kesinti bildirimlerini al
            return await _context.OutageNotification
                .Where(o => o.District == ilce && (o.EffectedNeighbourhoods.Contains(mahalle) || o.EffectedNeighbourhoods.StartsWith(mahalle + " |") || o.EffectedNeighbourhoods.EndsWith(" | " + mahalle) || o.EffectedNeighbourhoods.EndsWith(" | " + mahalle + " |")))
                .Where(o => o.Date.Date == tarih.Date)
                .ToListAsync();
        }

        public async Task<List<OutageNotification>> GetUnprocessedOutagesAsync()
        {
            var unprocessedOutages = await _context.OutageNotification
                .Where(o => !o.IsProcessed).ToListAsync();

            if (unprocessedOutages == null || !unprocessedOutages.Any())
            {
                // Veri dönmediği durumda yapılacak işlemler
                return new List<OutageNotification>(); // veya başka bir işlem
            }

            return unprocessedOutages;
        }

        public IEnumerable<OutageNotification> GetAll()
        {
            return _context.OutageNotification.ToList();
        }

        public IEnumerable<string> GetDistricts()
        {
            return _context.OutageNotification
                .Select(o => o.District)
                .Distinct()
                .ToList();
        }

        public IEnumerable<string> GetDistinctDates()
        {
            return _context.OutageNotification
                .Select(o => o.Date.ToString("yyyy-MM-dd")) // Tarih formatını ayarlayın
                .Distinct()
                .ToList();
        }

        public IEnumerable<OutageNotification> GetOutagesByDateAndDistrict(DateTime selectedDate, string district)
        {
            return _context.OutageNotification
                .Where(o => o.Date.Date == selectedDate.Date && o.District == district)
                .ToList();
        }
    }
}