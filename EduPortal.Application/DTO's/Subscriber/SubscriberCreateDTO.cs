using EduPortal.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduPortal.Application.DTO_s.Subscriber
{
    public class SubscriberCreateDTO : BaseModelDto
    {
        public int Id { get; set; }
        [BindProperty]
        public string SubscriberType { get; set; }
        [MaxLength(100)]
        public string PhoneNumber { get; set; }
        public string CounterNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
