using Microsoft.AspNetCore.Components.Forms;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class BlobFile
    {
        public Blob Blob { get; set; }
        public int BlobId { get; set; }
        public int BlobTypeId { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileSource { get; set; }
        public Stream FileStream { get; set; }
        public string[] AllowedExtensions { get; set; }

        [Required]
        [FileValidation(new[] { ".png", ".jpg", ".jpeg" })]
        public IBrowserFile BrowserFile { get; set; }
    }

    public class FileValidationAttribute : ValidationAttribute
    {
        public FileValidationAttribute(string[] allowedExtensions)
        {
            AllowedExtensions = allowedExtensions;
        }

        private string[] AllowedExtensions { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (IBrowserFile)value;

            var extension = Path.GetExtension(file.Name);

            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult($"File must have one of the following extensions: {string.Join(", ", AllowedExtensions)}.", new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
