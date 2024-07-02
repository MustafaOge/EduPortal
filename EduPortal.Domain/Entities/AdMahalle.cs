using System.ComponentModel.DataAnnotations;

namespace EduPortal.Domain.Entities
{
    public class Ad_Mahalle
    {
        [Key]
        public int mahalleKimlikNo { get; set; }
        public string adi { get; set; }
        public int ilceKimlikNo { get; set; }
    }
}
