namespace EduPortal.Application.Interfaces.Services
{
    public interface IFakeDataService
    {
        void CreateFakeSubsIndividualData();
        Task CreateFakeData();
        void CreateFakeInvoiceData();
        Task CreateCounterNumber();
    }
}
