using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Ad_DisKapi
    {
        [Key]
        public long disKapiKimlikNo { get; set; }
        public string adi { get; set; }
        public long sokakKimlikNo { get; set; }
    }
}
