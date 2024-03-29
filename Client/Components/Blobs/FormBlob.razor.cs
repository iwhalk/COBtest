﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using Obra.Client.Models;
using Obra.Client.Interfaces;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Fast.Components.FluentUI;
using System.Runtime.CompilerServices;

namespace Obra.Client.Components.Blobs
{
    public partial class FormBlob : ComponentBase
    {
        [Parameter]
        public Blob InputBlob { get; set; }
        [Parameter]
        public ICollection<Blob> InputBlobs { get; set; }

        [Parameter]
        public string[] AllowedExtensions { get; set; } = new[] { ".png", ".jpg", ".jpeg" };
        [Parameter]
        public int MaxAllowedSize { get; set; } = 2097152;
        [Parameter]
        public EventCallback<Blob> AddedBlob { get; set; }
        [Parameter]
        public EventCallback<int> OnImageClick { get; set; }

        [Parameter] public bool IsDisabled { get; set; }
        [Parameter] public string ColorSpan { get; set; }
        [Parameter] public string Cursor { get; set; }

        private readonly IBlobsService _blobService;
        public BlobFile CurrentBlobFile { get; set; }
        public ICollection<Blob> CurrentBlobs { get; set; }
        public string ValidationError { get; private set; }

        [Parameter] public EventCallback<bool> ShowModalWarning { get; set; }

        public EditContext CurrentBlobFileEditContext;
        private FluentDialog? MyFluentDialog;        

        public bool Loading { get; set; } = false;

        public FormBlob(IBlobsService blobService)
        {
            _blobService = blobService;
        }

        private async void InvokeValidNumberPhotos()
        {
            if(CurrentBlobs?.Count >= 3)
            {
                await ShowModalWarning.InvokeAsync(true);
            }
            return;
        }

        protected override async Task OnInitializedAsync()
        {
            CurrentBlobs = InputBlobs ?? new List<Blob>();

            CurrentBlobFile = new BlobFile()
            {
                Blob = InputBlob ?? new()
            };
            CurrentBlobFileEditContext = new EditContext(CurrentBlobFile);
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                MyFluentDialog!.Hide();
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

        

            Loading = true;
            CurrentBlobFile.BrowserFile = eventArgs.File;
            CurrentBlobFileEditContext.NotifyFieldChanged(FieldIdentifier.Create(() => CurrentBlobFile.BrowserFile));

            if (CurrentBlobFileEditContext.Validate())
            {
                //validationError = "";

                using var stream = eventArgs.File.OpenReadStream(maxAllowedSize: 52428800);
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
                    await AddedBlob.InvokeAsync(newBlob);
                }
                StateHasChanged();

                //BlobService.BlobFiles.Add(CurrentBlobFile);
            }

            Loading = false;
        }
        private void OnClose()
        {
            MyFluentDialog!.Hide();

        }
        private async Task OnClicked(int IdBlob)
        {
            await OnImageClick.InvokeAsync(IdBlob);
        }        
        private bool FileValidation(IBrowserFile file)
        {
            var extension = Path.GetExtension(file.Name);
            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                ValidationError = $"File must have one of the following extensions: {string.Join(", ", AllowedExtensions)}.";
                return false;
            }
            if (file.Size > MaxAllowedSize)
            {
                ValidationError = $"Maximum allowed file size is: {MaxAllowedSize / 1048576} MB.";
                return false;
            }
            return true;
        }
    }
}
