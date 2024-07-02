namespace EduPortal.Application.DTO_s.Subscriber
{
    public class SubsIndividualDto : SubscriberCreateDTO
    {
        public string NameSurname { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
