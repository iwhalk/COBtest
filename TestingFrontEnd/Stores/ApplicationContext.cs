using SharedLibrary.Models;
using static FrontEnd.Components.HeaderReceptionCertificatePendingOrHistorical;

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
        private ReceptionCertificate receptionCertificate;
        private List<Area> areasList;
        private List<Description> descriptionsList;
        private List<Feature> featuresList;
        private List<PropertyType> propertyTypesList;
        private List<ActasRecepcion> actasRecepcionsList;
        private List<Blob> BlobsList;
        private int typeReceptionCertificate;
        private int rowNumberPaginationCurrent;
        private int maxNumberPagination;
        private FilterReceptionCertificate currentFilterPagination;
        private List<int> listPageInPaginate;
        private string typeHistoricalOrPending;
        private List<AspNetUser> listUserAsp;
        private AspNetUser currentUser;
        private List<Area> currentAreasList;
        private bool completed;

        public List<AspNetUser> ListUserAsp
        {
            get => listUserAsp;
            set
            {                
                listUserAsp = value;
                NotifyStateChanged();
            }
        }
        public string TypeHistoricalOrPending
        {
            get => typeHistoricalOrPending;
            set
            {
                typeHistoricalOrPending = value;
                NotifyStateChanged();
            }
        }
        public List<int> ListPageInPaginate
        {
            get => listPageInPaginate;
            set
            {
                listPageInPaginate = value;
                NotifyStateChanged();
            }
        }
        public FilterReceptionCertificate CurrentFilterPagination
        {
            get => currentFilterPagination;
            set
            {
                currentFilterPagination = value;
                NotifyStateChanged();
            }
        }
        public int MaxNumberPagination
        {
            get => maxNumberPagination;
            set
            {
                maxNumberPagination = value;
                NotifyStateChanged();
            }
        }
        public int NumberPaginationCurrent
        {
            get => rowNumberPaginationCurrent;
            set
            {
                rowNumberPaginationCurrent = value;
                NotifyStateChanged();
            }
        }
        public int TypeReceptionCertificate
        {
            get => typeReceptionCertificate; 
            set
            {
                typeReceptionCertificate = value;
                NotifyStateChanged();
            }
        }

        public Tenant CurrentTenant
        {
            get => tenant;
            set
            {
                tenant = value;
                NotifyStateChanged();
            }
        }
        public List<Tenant> TenantList
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
        public List<Lessor> LessorList
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
        public List<Property> PropertyList
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
        public List<Service> ServiceList
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
        public List<Description> DescriptionList
        {
            get => descriptionsList;
            set
            {
                descriptionsList = value;
                NotifyStateChanged();
            }
        }
        public List<Feature> FeatureList
        {
            get => featuresList;
            set
            {
                featuresList = value;
                NotifyStateChanged();
            }
        }
        public List<PropertyType> PropertyTypeList
        {
            get => propertyTypesList;
            set
            {
                propertyTypesList = value;
                NotifyStateChanged();
            }
        }
        public ReceptionCertificate CurrentReceptionCertificate
        {
            get => receptionCertificate;
            set
            {
                receptionCertificate = value;
                NotifyStateChanged();
            }
        }

        public List<ActasRecepcion> ActasRecepcionList
        {
            get => actasRecepcionsList;
            set
            {
                actasRecepcionsList = value;
                NotifyStateChanged();
            }
        }

        public List<Blob> Blobs
        {
            get => BlobsList;
            set
            {
                BlobsList = value;
                NotifyStateChanged();
            }
        }
        public AspNetUser CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                NotifyStateChanged();
            }
        }
        public List<Area> CurrentAreasList
        {
            get => currentAreasList;
            set
            {
                currentAreasList = value;
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

        public bool Completed
        {
            get => completed;
            set
            {
                completed = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
