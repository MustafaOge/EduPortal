namespace EduPortal.Domain.Entities
{
    public class SubsIndividual : Subscriber
    {
        public string NameSurname { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdentityNumber { get; set; }
    }
}
