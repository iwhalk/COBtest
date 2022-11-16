using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using FrontEnd.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Components.Blobs
{
    public partial class FormBlob : ComponentBase
    {
        [Parameter]
        public Blob InputBlob { get; set; }
        [Parameter]
        public string[] AllowedExtensions { get; set; } = new[] { ".png", ".jpg" };
        [Parameter]
        public int MaxAllowedSize { get; set; } = 2097152;


        private readonly IBlobService _blobService;

        public BlobFile CurrentBlobFile { get; set; }
        public EditContext CurrentBlobFileEditContext;

        private string FileName = "";

        public FormBlob(IBlobService blobService)
        {
            _blobService = blobService;
        }

        protected override void OnInitialized()
        {
        }

        protected override void OnParametersSet()
        {
            CurrentBlobFile = new BlobFile()
            {
                Blob = InputBlob ?? new()
            };
            CurrentBlobFileEditContext = new EditContext(CurrentBlobFile);
        }

        private async Task OnChangeAsync(InputFileChangeEventArgs eventArgs)
        {
            CurrentBlobFile.BrowserFile = eventArgs.File;
            CurrentBlobFileEditContext.NotifyFieldChanged(FieldIdentifier.Create(() => CurrentBlobFile.BrowserFile));

            if (CurrentBlobFileEditContext.Validate())
            {
                //validationError = "";

                using var stream = eventArgs.File.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                CurrentBlobFile.FileStream = new MemoryStream(memoryStream.ToArray());

                CurrentBlobFile.Blob.BlodName = eventArgs.File.Name;
                CurrentBlobFile.Blob.BlobSize = CurrentBlobFile.FileStream.Length.ToString();
                CurrentBlobFile.Blob.BlodTypeId = "1";
                CurrentBlobFile.Blob.ContentType = eventArgs.File.ContentType;

                var res = await _blobService.PostBlobAsync(CurrentBlobFile);
                if (res != null)
                {
                    CurrentBlobFile.Blob.IdBlobs = res.IdBlobs;
                }
                //BlobService.BlobFiles.Add(CurrentBlobFile);
            }

        }

        //private bool FileValidation(IBrowserFile file)
        //{
        //    var extension = Path.GetExtension(file.Name);
        //    if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        //    {
        //        validationError = $"File must have one of the following extensions: {string.Join(", ", AllowedExtensions)}.";
        //        return false;
        //    }
        //    if (file.Size > MaxAllowedSize)
        //    {
        //        validationError = $"Maximum allowed file size is: {MaxAllowedSize/1048576} MB.";
        //        return false;
        //    }
        //    return true;
        //}
    }
}
