using ReportesData.Models;

namespace TestingFrontEnd.Stores
{
    public class ApplicationContext
    {
        public event Action OnChange;
        private int counter;
        private string errorMessage; 
        private UsuarioPlaza usuarioPlaza;
        private List<TypeDelegacion> delegaciones;
        private List<TypePlaza> plazas;
        private List<Personal> administradores;
        private List<Personal> encargadosTurno;
        private KeyValuePair<string, string>[] turnos;

        public int Counter
        {
            get => counter;
            set
            {
                counter = value;
                NotifyStateChanged();
            }
        }
        public UsuarioPlaza UsuarioPlaza
        {
            get => usuarioPlaza;
            set
            {
                usuarioPlaza = value;
                NotifyStateChanged();
            }
        }
        public List<TypePlaza> Plazas
        {
            get => plazas;
            set
            {
                plazas = value;
                NotifyStateChanged();
            }
        }
        public List<TypeDelegacion> Delegaciones
        {
            get => delegaciones;
            set
            {
                delegaciones = value;
                NotifyStateChanged();
            }
        }
        public List<Personal> Administradores
        {
            get => administradores;
            set
            {
                administradores = value;
                NotifyStateChanged();
            }
        }
        public List<Personal> EncargadosTurno
        {
            get => encargadosTurno;
            set
            {
                encargadosTurno = value;
                NotifyStateChanged();
            }
        }
        public KeyValuePair<string, string>[] Turnos
        {
            get => turnos;
            set
            {
                turnos = value;
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
