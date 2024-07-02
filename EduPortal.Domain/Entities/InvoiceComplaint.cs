namespace EduPortal.Domain.Entities
{
    public class InvoiceComplaint : BaseEntity<int>
    {
        public int InvoiceId { get; set; }
        public string Title { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
