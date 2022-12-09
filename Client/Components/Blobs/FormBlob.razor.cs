using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using Obra.Client.Models;
using Obra.Client.Interfaces;
using System.Text;
using System.Reflection.Metadata.Ecma335;

namespace Obra.Client.Components.Blobs
{
    public partial class FormBlob : ComponentBase
    {
        [Parameter]
        public Blob InputBlob { get; set; }
        [Parameter]
        public List<Blob> InputBlobs { get; set; }

        [Parameter]
        public string[] AllowedExtensions { get; set; } = new[] { ".png", ".jpg", ".jpeg" };
        [Parameter]
        public int MaxAllowedSize { get; set; } = 2097152;
        [Parameter]
        public EventCallback<Blob> AddedBlob { get; set; }
        [Parameter]
        public string HeigthContent { get; set; }
        [Parameter]
        public string SizeImg { get; set; }
        [Parameter]
        public EventCallback<Blob> CurrentBlob { get; set; }

        private readonly IBlobsService _blobService;
        public BlobFile CurrentBlobFile { get; set; }
        public List<BlobFile> CurrentBlobFiles { get; set; } = new();
        public List<Blob> CurrentBlobs { get; set; } = new();
        public ImageFile CurrentImageFile { get; set; } = new();
        public List<ImageFile> CurrentImageFiles { get; set; } = new();

        public EditContext CurrentBlobFileEditContext;

        public List<string> ListBase64Blobs { get; set; } = new();

        private string FileName = "";

        public FormBlob(IBlobsService blobService)
        {
            _blobService = blobService;
        }


        protected override async Task OnInitializedAsync()
        {
            //CurrentBlobs = InputBlobs ?? new List<Blob>();

            //CurrentBlobFile = new BlobFile()
            //{
            //    Blob = InputBlob ?? new()
            //};

            //CurrentBlobFileEditContext = new EditContext(CurrentBlobFile);
        }
        protected override async Task OnParametersSetAsync()
        {
            CurrentBlobs = InputBlobs ?? new List<Blob>();

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

                var bleb = new Blob()
                {
                    BlobName = eventArgs.File.Name,
                    BlobSize = memoryStream.Length.ToString(),
                    ContentType= eventArgs.File.ContentType,
                };

                CurrentBlobFile.Blob = bleb;
                CurrentBlobFile.FileStream = new MemoryStream(memoryStream.ToArray());
                CurrentBlobFile.FileName = eventArgs.File.Name;
                CurrentBlobFile.MimeType = MimeMapping.MimeUtility.GetMimeMapping(eventArgs.File.Name);

                //CurrentBlobFile.Blob.BlobName = eventArgs.File.Name;
                //CurrentBlobFile.Blob.BlobSize = CurrentBlobFile.FileStream.Length.ToString();
                //CurrentBlobFile.Blob.BlobTypeId = "1";
                //CurrentBlobFile.Blob.ContentType = eventArgs.File.ContentType;

                //CurrentImageFile.FileContent = memoryStream.ToArray();
                //CurrentImageFile.FileName = eventArgs.File.Name;
                //CurrentImageFile.MimeType = MimeMapping.MimeUtility.GetMimeMapping(eventArgs.File.Name);

                //ImageFile imageFile = new() { FileContent = CurrentImageFile.FileContent, FileName = CurrentImageFile.FileName, MimeType = CurrentImageFile.MimeType };
                //CurrentImageFiles.Add(imageFile);


                var newBlob = await _blobService.PostBlobAsync(CurrentBlobFile);
                if (newBlob != null)
                {
                    CurrentBlobFile.Blob = newBlob;
                    CurrentBlobs.Add(newBlob);
                    StateHasChanged();
                    await AddedBlob.InvokeAsync(newBlob);
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
