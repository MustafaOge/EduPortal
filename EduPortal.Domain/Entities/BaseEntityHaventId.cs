using EduPortal.Domain.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public abstract class BaseEntityCustom<TKey> : IEntity
    {

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public int CreatedByUser { get; set; }

        public int? UpdatedByUser { get; set; }
    }
}
