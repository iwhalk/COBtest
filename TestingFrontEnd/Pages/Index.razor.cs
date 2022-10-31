using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata.Ecma335;

namespace TestingFrontEnd.Pages
{
    public partial class Index : ComponentBase
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
