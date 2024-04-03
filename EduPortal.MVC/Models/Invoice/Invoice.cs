namespace EduPortal.MVC.Models.Invoice
{
    public class Invoice
    {
        public DateTime Date { get; set; }
        public int? SubscriberId { get; set; }
        public decimal Amount { get; set; }
        public string SubscriberType { get; set; }
        public DateTime ReadingDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsPaid { get; set; } // Fatura ödendi mi?
    }
}
