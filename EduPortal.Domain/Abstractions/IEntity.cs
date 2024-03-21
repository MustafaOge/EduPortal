using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Abstractions
{
    public interface IEntity
    { }
    //public abstract class IEntity
    //{
    //    public IEntity()
    //    {
    //        CreatedTime = null;
    //        IsDeleted = false;
    //        IsActive = true;
    //    }

    //    [Key]
    //    public int Id { get; set; }
    //    public DateTime? CreatedTime { get; set; }
    //    public DateTime? UpdatedTime { get; set; }
    //    public bool IsActive { get; set; }
    //    public bool IsDeleted { get; set; }

    //}
}
