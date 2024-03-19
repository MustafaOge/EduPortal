using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.DTO_s.User
{
    public class UserCreateDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
