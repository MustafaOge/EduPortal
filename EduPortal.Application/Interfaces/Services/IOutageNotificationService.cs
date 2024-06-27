using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;

namespace EduPortal.Application.Interfaces.Services
{
    public interface IOutageNotificationService
    {
        Task SendOutageMessageAsync();
        List<string> GetDistricts();
        List<string> GetDistinctDates();
        List<OutageNotification> GetOutagesByDateAndDistrict(DateTime selectedDate, string district);
    }
}
