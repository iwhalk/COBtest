using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ImageFile
    {
        public string? MimeType { get; set; }

        public byte[]? FileContent { get; set; }

        public string? FileName { get; set; }

        public string? Url { get; set; }

        public string? Base64Content
        {
            get
            {
                string? convertedContent = null;

                if (FileContent != null)
                {
                    convertedContent = $"data:{MimeType};base64,{Convert.ToBase64String(FileContent)}";
                }

                return convertedContent;
            }
        }

    }
}
