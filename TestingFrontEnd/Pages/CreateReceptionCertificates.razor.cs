using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace TestingFrontEnd.Pages
{
    public partial class CreateReceptionCertificates : ComponentBase
    {
        public bool ShowModalLessor { get; set; } = false;
        public bool ShowModalTenant { get; set; } = false;
        public bool ShowModalProperty { get; set; } = false;
        public Lessor? NewLessor { get; set; } = new Lessor { Name= "Luis", LastName = "EMiliano", EmailAddress = "luis@gmail.com", Cp = "14100", Colony = "Pedregal de Sn Nicolas", Delegation = "Tlalpan", PhoneNumber = "5556441051", Rfc = "3234235234", Street = "Sisal"};

        public void ChangeOpenModalLessor() => ShowModalLessor = ShowModalLessor ? false : true;
        public void ChangeOpenModalTenant() => ShowModalTenant = ShowModalTenant ? false : true;
        public void ChangeOpenModalProperty() => ShowModalProperty = ShowModalProperty ? false : true;
    }
}
