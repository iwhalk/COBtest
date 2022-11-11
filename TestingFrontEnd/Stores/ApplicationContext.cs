using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Stores
{
    public class ApplicationContext
    {
        public event Action OnChange;        
        private string errorMessage; 
        private List<Tenant> tenantList;
        private List<Lessor> lessorList;
        private List<Property> propertiesList;
        private List<Inventory> inventoriesList;
        private List<Service> serviceList;
        private Tenant tenant;
        private Lessor lessor;
        private Property property;
        private List<Area> areasList;
        private List<Description> descriptionsList;
        private List<Feature> featuresList;
        private List<PropertyType> propertyTypesList;
        private List<ActasRecepcion> actasRecepcionsList;

        public Tenant CurrentTenant
        {
            get => tenant;
            set
            {
                tenant = value;
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
        public Lessor CurrentLessor
        {
            get => lessor;
            set
            {
                lessor = value;
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
        public Property CurrentPropertys
        {
            get => property; 
            set
            {
                property = value;
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
        public List<Inventory> Inventory
        {
            get => inventoriesList;
            set
            {
                inventoriesList = value;
                NotifyStateChanged();
            }
        }
        public List<Service> Service
        {
            get => serviceList;
            set
            {
                serviceList = value;
                NotifyStateChanged();
            }
        }
        public List<Area> Area
        {
            get => areasList;
            set
            {
                areasList = value;
                NotifyStateChanged();
            }
        }
        public List<Description> Description
        {
            get => descriptionsList;
            set
            {
                descriptionsList = value;
                NotifyStateChanged();
            }
        }
        public List<Feature> Feature
        {
            get => featuresList;
            set
            {
                featuresList = value;
                NotifyStateChanged();
            }
        }
        public List<PropertyType> PropertyType
        {
            get => propertyTypesList;
            set
            {
                propertyTypesList = value;
                NotifyStateChanged();
            }
        }
        public List<ActasRecepcion> ActasRecepcion
        {
            get => actasRecepcionsList;
            set
            {
                actasRecepcionsList = value;
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
