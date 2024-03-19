namespace EduPortal.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string SubcriberType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }
}
