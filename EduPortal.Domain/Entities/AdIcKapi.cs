using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPortal.Domain.Entities
{
    public class Ad_IcKapi : BaseEntityCustom<int>
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
        public int counterNumber { get; set; }

        [ForeignKey("disKapiKimlikNo")]
        public virtual Ad_DisKapi Ad_DisKapi { get; set; }

        [ForeignKey("sokakKimlikNo")]
        public virtual Ad_Sokak Ad_Sokak { get; set; }

        [ForeignKey("mahalleKimlikNo")]
        public virtual Ad_Mahalle Ad_Mahalle { get; set; }

        [ForeignKey("ilceKimlikNo")]
        public virtual Ad_Ilce Ad_Ilce { get; set; }

        [ForeignKey("counterNumber")]
        public virtual Ad_Sayac Ad_Sayac { get; set; }
    }
}
