using Shared.Models;

namespace TestingFrontEnd.Stores
{
    public class ApplicationContext
    {
        public event Action OnChange;
        private int counter;
        private string errorMessage; 
        private List<Tenant> tenantList;
        private List<Lessor> lessorList;
        private List<Property> propertiesList;

        public int Counter
        {
            get => counter;
            set
            {
                counter = value;
                NotifyStateChanged();
            }
        }

        public List<Tenant> Tenant
        {
            get => tenantList;
            set
            {
                tenantList = value;
                NotifyStateChanged();
            }
        }
        public List<Lessor> Lessor
        {
            get => lessorList;
            set
            {
                lessorList = value;
                NotifyStateChanged();
            }
        }
        public List<Property> Property
        {
            get => propertiesList;
            set
            {
                propertiesList = value;
                NotifyStateChanged();
            }
        }

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
