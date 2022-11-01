using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Pages
{
    public partial class IndexReceptionCertificates : ComponentBase
    {
        public bool OpenModal { get; set; } = false;
        public string TypeIndex { get; set; } = "/";
        public void ChangeOpenModal() => OpenModal = OpenModal ? false : true;
        public void ChangeTypeIndex(string newTypeIndex)
        {
            TypeIndex = newTypeIndex;
        }
    }
}
