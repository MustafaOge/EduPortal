using NToastNotify;


namespace EduPortal.MVC.Helpers
{
    public class ToastHelper(IToastNotification toastService)
    {
        public void AddSuccessToastMessage(string message)
        {
            toastService.AddSuccessToastMessage(message, new ToastrOptions { Title = "Başarılı!" });
        }

        public void AddErrorToastMessage(string message)
        {
            toastService.AddErrorToastMessage(message, new ToastrOptions { Title = "Başarısız!" });
        }
    }
}
