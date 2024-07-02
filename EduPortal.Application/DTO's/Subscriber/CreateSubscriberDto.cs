namespace EduPortal.Application.DTO_s.Subscriber
{
    public class CreateSubscriberDto
    {
        public string SubscriberType { get; set; }
        public CreateIndividualDto Individual { get; set; }
        public CreateCorporateDto Corporate { get; set; }
    }

}
