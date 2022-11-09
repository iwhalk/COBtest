﻿using Shared.Models;
using SharedLibrary.Models;

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
        private List<Inventory> inventoriesList;
        private List<Service> serviceList;

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
