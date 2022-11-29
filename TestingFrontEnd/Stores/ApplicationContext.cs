using SharedLibrary.Models;
using static FrontEnd.Components.HeaderReceptionCertificatePendingOrHistorical;

namespace FrontEnd.Stores
{
    public class ApplicationContext
    {
        public event Action OnChange;        
        private string errorMessage; 

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
