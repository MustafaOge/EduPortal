using EduPortal.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.DTO_s.Invoice
{
    public class InvoiceComplaintCreateDto : BaseModelDto
    {
        public int InvoiceId { get; set; }
        public string Title { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
