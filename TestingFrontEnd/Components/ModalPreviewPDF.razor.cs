using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
namespace FrontEnd.Components
{
    public partial class ModalPreviewPDF : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; }
        [Parameter]
        public EventCallback CloseModalLessor { get; set; }
        [Parameter]
        public byte[] Content { get; set; }
        [Parameter]
        public string PdfName { get; set; }
        private string? BlobUrl { get; set; }

        private readonly IJSInProcessRuntime _js;

        public ModalPreviewPDF(IJSInProcessRuntime js)
        {
            _js = js;
        }
        protected override void OnInitialized()
        {
            if (Content != null)
            { 
                BlobUrl = _js.Invoke<string>("CreateObjectURL", Content, PdfName ?? "Pdf.pdf", "application/pdf");
            }
        }

        public void Dispose()
        {
            _js.Invoke<string>("RevokeObjectURL", BlobUrl);
            BlobUrl = null;
        }
    }
}
