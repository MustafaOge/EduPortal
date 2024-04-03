using System.ComponentModel.DataAnnotations;

namespace EduPortal.MVC.Models.Subscriber
{
    public class SubsCorporate : SubscriberViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kurum Adı")]
        [Required(ErrorMessage = "Kurum Adı Alanı Zorunludur")]
        [MaxLength(200)]
        public string CorporateName { get; set; }

        [MinLength(10, ErrorMessage = "Geçersiz Vergi Numarası"), MaxLength(10, ErrorMessage = "Geçersiz Vergi Numarası")]
        [Display(Name = "Vergi Numarası")]
        [Required(ErrorMessage = "Vergi Numarası Alanı Zorunludur")]
        public string TaxIdNumber { get; set; }

        [Display(Name = "Abone Türü")]
        public string SubscriberType { get; set; }

        [Display(Name = "Telefon Numarası")]
        [MinLength(11, ErrorMessage = "Geçersiz Telefon Numarası"), MaxLength(11, ErrorMessage = "Geçersiz Telefon Numarası")]
        [Required(ErrorMessage = "Telefon Numarası Alanı Zorunludur")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Sayaç Numarası")]
        [Required(ErrorMessage = "Sayaç Numarası Alanı Zorunludur")]
        public string CounterNumber { get; set; }

        [Display(Name = "E-posta Adresi")]
        [Required(ErrorMessage = "Email Adresi Alanı Zorunludur")]
        public string Email { get; set; }
    }
}
