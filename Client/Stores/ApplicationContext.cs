using SharedLibrary.Models;

namespace Obra.Client.Stores
{
    public class ApplicationContext
    {
        public event Action OnChange;
        private string errorMessage;

        private List<Building> buildingsList;
        private List<Area> areasList;
        private List<Activity> activitiesList;
        private List<Apartment> apartmentsList;
        private List<Element> elementsList;
        private List<SubElement> subElementsList;
        private List<ProgressLog> progressLogsList;
        private List<Blob> blobList;
        private List<ProgressReport> progressReportList;

        public List<Building> Building
        {
            get => buildingsList;
            set
            {
                buildingsList = value;
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
        public List<Activity> Activity
        {
            get => activitiesList;
            set
            {
                activitiesList = value;
                NotifyStateChanged();
            }
        }
        public List<Apartment> Apartment
        {
            get => apartmentsList;
            set
            {
                apartmentsList = value;
                NotifyStateChanged();
            }
        }
        public List<Element> Element
        {
            get => elementsList;
            set
            {
                elementsList = value;
                NotifyStateChanged();
            }
        }
        public List<SubElement> SubElement
        {
            get => subElementsList;
            set
            {
                subElementsList = value;
                NotifyStateChanged();
            }
        }
        public List<ProgressLog> ProgressLog
        {
            get => progressLogsList;
            set
            {
                progressLogsList = value;
                NotifyStateChanged();
            }
        }
        public List<Blob> Blob
        {
            get => blobList;
            set
            {
                blobList = value;
                NotifyStateChanged();
            }
        }
        public List<ProgressReport> ProgressReport
        {
            get => progressReportList;
            set
            {
                progressReportList = value;
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
