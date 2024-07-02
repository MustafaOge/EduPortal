namespace EduPortal.Domain.Entities
{
    public class Consumer : BaseEntity<int>
    {        
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
