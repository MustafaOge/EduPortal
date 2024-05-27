using EduPortal.Application.DTO_s.Subscriber;
using FluentValidation;

namespace EduPortal.Application.Validations.Subscriber
{
    public class CreateSubsCoporateDtoValidator : AbstractValidator<CreateCorporateDto>
    {
        public CreateSubsCoporateDtoValidator()
        {
            RuleFor(dto => dto.CorporateName)
                .NotEmpty().WithMessage("Kurum Adı Boş Bırakılamaz.")
                .MaximumLength(255).WithMessage("Kurum Adı 255 karakterden fazla olamaz.");

            RuleFor(dto => dto.TaxIdNumber)
                .NotEmpty().WithMessage("Vergi Kimlik Numarası Boş Bırakılamaz.")
                .Matches("^[0-9]*$").WithMessage("Vergi Kimlik Numarası sadece rakamlardan oluşmalıdır.")
                .Length(10).WithMessage("Vergi Kimlik Numarası 10 karakter uzunluğunda olmalıdır.");

            RuleFor(dto => dto.SubscriberType)
                .NotEmpty().WithMessage("Abone Türü Boş Bırakılamaz.");

            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("Telefon Numarası Boş Bırakılamaz.")
                .Matches(@"^[0-9]*$").WithMessage("Telefon Numarası sadece rakamlardan oluşmalıdır.")
                .Length(11).WithMessage("Telefon Numarası 11 karakter uzunluğunda olmalıdır.");

            //RuleFor(dto => dto.CounterNumber)
            //    .NotEmpty().WithMessage("Sayacı Numarası Boş Bırakılamaz.")
            //    .Matches(@"^[0-9]*$").WithMessage("Sayacı Numarası sadece rakamlardan oluşmalıdır.")
            //    .Length(7).WithMessage("Sayacı Numarası 7 karakter uzunluğunda olmalıdır.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("E-Posta Adresi Boş Bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir E-Posta Adresi girilmelidir.");
        }
    }
}
