using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository :IGenericRepository<Invoice,int>
    {
        Task<bool> AnyInvoiceAsync();


    }
}



/// <summary>
/// Pays the debt of the selected invoice.
/// </summary>
/// <param name="id">The ID of the selected invoice</param>
/// <returns>A task representing the asynchronous operation</returns>
//Task PayInvoice(int id);