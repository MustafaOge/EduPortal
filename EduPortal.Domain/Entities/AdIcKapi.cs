using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Ad_IcKapi
    {
        [Key]
        public long icKapiKimlikNo { get; set; }
        public string adresNo { get; set; }
        public long disKapiKimlikNo { get; set; }
        public long sokakKimlikNo { get; set; }
        public int mahalleKimlikNo { get; set; }
        public int ilceKimlikNo { get; set; }
        public int katNo { get; set; }
        public int icKapiNo { get; set; }
    }

}
