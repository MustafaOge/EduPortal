using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Ad_Sayac
    {
        [Key]
        public int counterNumber { get; set; }
        public long icKapiKimlikNo { get; set; }
        public bool active { get; set; }
    }

}
