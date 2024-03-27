using System.ComponentModel.DataAnnotations;

namespace EduPortal.MVC.Models.Subscriber
{
    public class SubscriberModel
    {
        public int Id { get; set; }
        [StringLength(12, MinimumLength = 10, ErrorMessage = "11 karakter olacakla")]
        [Required(ErrorMessage = "Ad Soyad Alanı Zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string NameSurname { get; set; }

        [Display(Name = "Kimlik Numarası")]
        [Required(ErrorMessage = "Kimlik Numarası Alanı Zorunludur")]
        public string IdentityNumber { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [Required(ErrorMessage = "Doğum Tarihi Alanı Zorunludur")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Telefon Numarası")]
        [MinLength(11, ErrorMessage = "Geçersiz Telefon Numarası"), MaxLength(11, ErrorMessage = "Geçersiz Telefon Numarası")]
        [Required(ErrorMessage = "Telefon Numarası Alanı Zorunludur")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Sayaç Numarası")]
        [Required(ErrorMessage = "Sayaç Numarası Alanı Zorunludur")]
        public string CounterNumber { get; set; }

        [Display(Name = "E-posta Adresi")]
        [Required(ErrorMessage = "Email Alanı Zorunludur")]

        // Eğer gerekiyorsa, Email adresi için validation ekleme burada yapılabilir.
        public string Email { get; set; }

        [Display(Name = "Abone Türü")]
        public string SubscriberType { get; set; }

        public string CorporateName { get; set; }

        [Display(Name = "E-posta Adresi")]
        public string TaxIdNumber { get; set; }
    }
}
