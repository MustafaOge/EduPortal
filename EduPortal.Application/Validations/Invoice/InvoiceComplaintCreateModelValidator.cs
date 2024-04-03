using EduPortal.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Validations.Invoice
{
    public class InvoiceComplaintCreateModelValidator : AbstractValidator<InvoiceComplaint>
    {
        public InvoiceComplaintCreateModelValidator()
        {
            RuleFor(x => x.InvoiceId).NotEmpty().WithMessage("Fatura ID gereklidir.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("İtiraz başlığı gereklidir.");
            RuleFor(x => x.Reason).NotEmpty().WithMessage("İtiraz nedeni gereklidir.");
        }
    }
}
