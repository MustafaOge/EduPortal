using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface IFakeDataService
    {

        void CreateFakeSubsIndividualData();
        Task CreateFakeData();

        void CreateFakeInvoiceData();


    }
}
