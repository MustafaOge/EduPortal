using System.ComponentModel.DataAnnotations;

namespace EduPortal.MVC.Models.Subscriber
{
    public class SubsIndividual
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad Alanı Zorunludur")]

        public string NameSurname { get; set; }


        [Required(ErrorMessage = "Kimlik Numarası Alanı Zorunludur")]

        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Doğum Tarihi Alanı Zorunludur")]

        public DateTime BirthDate { get; set; }

        [MinLength(11, ErrorMessage = "Gerçersiz Telefon Numarası"), MaxLength(11, ErrorMessage = "Gerçersiz Telefon Numarası")]
        [Required(ErrorMessage = "Telefon Numarası Alanı Zorunludur")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Sayaç Numarası Alanı Zorunludur")]

        public int CounterNumber { get; set; }

        [Required(ErrorMessage = "Email Adresi Alanı Zorunludur")]

        public string Email { get; set; }
        public string SubscriberType { get; set; }

    }
}
