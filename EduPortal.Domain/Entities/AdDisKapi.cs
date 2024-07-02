using System.ComponentModel.DataAnnotations;

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
