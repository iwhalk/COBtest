namespace TestingFrontEnd.Stores
{
    public class ApplicationContext
    {
        public event Action OnChange;
        private int counter;
        private string errorMessage; 

        public int Counter
        {
            get => counter;
            set
            {
                counter = value;
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
