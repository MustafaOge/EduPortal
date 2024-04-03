using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface IFakeDataService
    {

        void CreateFakeSubsInvoicesData(); // Sahte fatura verileri oluşturur, başarı durumunu döndürür
        void CreateFakeSubsIndividualData();

        void CreateFakeInvoiceData();
    }
}
