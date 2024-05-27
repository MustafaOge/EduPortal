using System.ComponentModel.DataAnnotations;

namespace EduPortal.MVC.Models.Subscriber
{
    public class SubsIndividual
    {
        [Display(Name = "NameSurname")]
        [Required(ErrorMessage = "Ad Soyad Alanı Zorunludur")]
        [MaxLength(200)]
        public string NameSurname { get; set; }

        [Display(Name = "Kimlik Numarası")]
        [MinLength(11, ErrorMessage = "Geçersiz Kimlik Numarası"), MaxLength(11, ErrorMessage = "Geçersiz Kimlik Numarası")]
        [Required(ErrorMessage = "Kimlik Numarası Alanı Zorunludur")]
        public string IdentityNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string SubscriberType { get; set; }

        [Display(Name = "Telefon Numarası")]
        [MinLength(11, ErrorMessage = "Geçersiz Telefon Numarası"), MaxLength(11, ErrorMessage = "Geçersiz Telefon Numarası")]
        [Required(ErrorMessage = "Telefon Numarası Alanı Zorunludur")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Sayaç Numarası")]
        public string CounterNumber { get; set; } // Buradaki nullable (?) kaldırıldı.

        [Display(Name = "E-posta Adresi")]
        [Required(ErrorMessage = "Email Adresi Alanı Zorunludur")]
        public string Email { get; set; }

        public string InternalDoorNumber { get; set; }

    }
}
//[Required(ErrorMessage = "Sayaç Numarası Alanı Zorunludur")]
//[MinLength(7, ErrorMessage = "Geçersiz Sayaç Numarası"), MaxLength(7, ErrorMessage = "Geçersiz Sayaç Numarası")]
