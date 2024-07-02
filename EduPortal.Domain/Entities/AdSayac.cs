using System.ComponentModel.DataAnnotations;

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
