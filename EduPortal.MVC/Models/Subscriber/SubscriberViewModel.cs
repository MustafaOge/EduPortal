﻿using EduPortal.Application.DTO_s.Subscriber;

namespace EduPortal.MVC.Models.Subscriber
{
    public class SubscriberViewModel
    {
        public SubsIndividualDto IndividualList { get; set; }
        public List<SubsCorporateDto> CorporateList { get; set; }
    }
}
