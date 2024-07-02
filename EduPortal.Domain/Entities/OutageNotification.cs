namespace EduPortal.Domain.Entities
{
    public class OutageNotification : BaseEntity<int>
    {
        public string? Province { get; set; }
        public string? District { get; set; }
        public DateTime Date { get; set; }
        public string? DistributionCompanyName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Reason { get; set; }
        public string? EffectedNeighbourhoods { get; set; }
        public int EffectedSubscribers { get; set; }
        public int HourlyLoadAvg { get; set; }
        public List<int> MahalleKimlikNumaralari { get; set; } = new List<int>();
        public bool IsProcessed { get; set; }
    }
}
