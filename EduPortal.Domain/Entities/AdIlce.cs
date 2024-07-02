using System.ComponentModel.DataAnnotations;

namespace EduPortal.Domain.Entities
{
    public class Ad_Ilce
    {
        [Key]
        public int ilceKimlikNo { get; set; }
        public string adi { get; set; }
    }
}
