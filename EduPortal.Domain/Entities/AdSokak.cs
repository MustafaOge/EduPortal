using System.ComponentModel.DataAnnotations;

namespace EduPortal.Domain.Entities
{
    public class Ad_Sokak
    {
        [Key]
        public long sokakKimlikNo { get; set; }
        public string adi { get; set; }
        public int mahalleKimlikNo { get; set; }
    }
}
