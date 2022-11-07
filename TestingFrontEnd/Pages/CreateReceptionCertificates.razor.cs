using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Pages
{
    public partial class CreateReceptionCertificates : ComponentBase
    {
        public bool ShowModalLessor { get; set; } = false;
        public bool ShowModalTenant { get; set; } = false;
        public bool ShowModalProperty { get; set; } = false;

        public void ChangeOpenModalLessor() => ShowModalLessor = ShowModalLessor ? false : true;
        public void ChangeOpenModalTenant() => ShowModalTenant = ShowModalTenant ? false : true;
        public void ChangeOpenModalProperty() => ShowModalProperty = ShowModalProperty ? false : true;
    }
}
