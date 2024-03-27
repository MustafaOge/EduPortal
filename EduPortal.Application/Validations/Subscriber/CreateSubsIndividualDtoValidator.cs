using EduPortal.Application.DTO_s.Subscriber;
using FluentValidation;
using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Validations.Subscriber
{
    public class CreateSubsIndividualDtoValidator : AbstractValidator<CreateIndividualDto>
    {
        public CreateSubsIndividualDtoValidator()
                {
             RuleFor(dto => dto.NameSurname)
                .NotEmpty().WithMessage("Abone İsmi Boş Geçilemez.")
                .MinimumLength(5).WithMessage("Abone isim soyismi 5 karakterden az olamaz.")
                .MaximumLength(255).WithMessage("Abone isim soyismi 255 karakterden fazla olamaz.");

            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("Telefon Numarası Boş Geçilemez.")
                .Length(11).WithMessage("telefon numarası 11 karakter uzunluğunda olmalıdır.");


            RuleFor(dto => dto.CounterNumber)
                .NotEmpty().WithMessage("Sayacı Numarası Boş Geçilemez.")
                .Length(7).WithMessage("Sayacı numarası 7 karakter uzunluğunda olmalıdır.");

            RuleFor(dto => dto.IdentityNumber)
                .NotEmpty().WithMessage("Kimlik Numarası Boş Geçilemez.")
                .Length(11).WithMessage("Kimlik numarası 11 karakter uzunluğunda olmalıdır.")
                .Matches("^[1-9][0-9]{10}$").WithMessage("Geçersiz kimlik numarası formatı."); 

            RuleFor(dto => dto.BirthDate)
                .NotEmpty().WithMessage("Doğum Tarihi Boş Geçilemez.")
                .Must(BeAValidDate).WithMessage("Geçersiz doğum tarihi formatı.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("E-Posta Adresi Boş Geçilemez.")
                .EmailAddress().WithMessage("Geçersiz e-posta adresi formatı.");




            RuleFor(dto => dto.SubscriberType).NotEmpty().WithMessage("Abone Tipi Boş Geçilemez.");



        }
 
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

    }
}
