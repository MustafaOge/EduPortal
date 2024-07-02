namespace EduPortal.Domain.Entities
{
    public class OutboxMessage : BaseEntity<int>
    {
        public DateTime OccurredOn { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public bool IsProcessed { get; set; }

    }
}
