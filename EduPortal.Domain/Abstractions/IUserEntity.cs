﻿using EduPortal.Models.Enums;

namespace EduPortal.Domain.Abstractions
{
    public interface IUserEntity
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }
    }
}
