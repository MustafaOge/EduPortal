﻿using EduPortal.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Subscriber: BaseEntity
    {
        public int Id { get; set; }
        public string SubscriberContractNumber { get; set; } 
    }
}
// Authentşcatşon cookies