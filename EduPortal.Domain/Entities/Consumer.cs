using EduPortal.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Consumer : BaseEntity
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
