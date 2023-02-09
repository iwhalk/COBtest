﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System.IO;
using Org.BouncyCastle.Utilities.Zlib;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Net;
using System.Reflection.Metadata;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats;
using System.Text.RegularExpressions;
using NuGet.ContentModel;
using Microsoft.CodeAnalysis;
using System.Web;
using System.Data.Entity;
using System.Buffers.Text;

namespace ReportesObra.Services
{
    public class ImageService : IImageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ObraDbContext _dbContext;

        public ImageService(ObraDbContext dbContext, BlobServiceClient blobServiceClient)
        {
            _dbContext = dbContext;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<bool> MixImage(ImageData imageData)
        {
            try
            {
                WebClient clientHeader = new WebClient();

                var toByteArray = clientHeader.DownloadData(imageData.BlobUri);
                var toBase64String = Convert.ToBase64String(toByteArray);
                MemoryStream streamHeader1 = new MemoryStream((Convert.FromBase64String(toBase64String)));
                Match extention = Regex.Match(imageData.BlobUri, "\\.\\w{3,4}($|\\?)");
                var decoder = GetDecoder(extention.Value);
                var encoder = GetEncoder(extention.Value);

                Stream? streamDraw = new MemoryStream(Convert.FromBase64String(imageData.StringBase64.Split(',')[1]));

                IImageInfo imageInfo = Image.Identify(streamDraw);
                streamDraw.Position = 0;

                using (var output = new MemoryStream())
                using (var imageToDraw = SixLabors.ImageSharp.Image.Load(streamHeader1))
                using (var draw = SixLabors.ImageSharp.Image.Load<Rgba64>(streamDraw))
                using (Image<Rgba32> outputImage = new Image<Rgba32>(imageInfo.Width, imageInfo.Height)) // create output image of the correct dimensions
                {
                    //sello.Mutate(o => o.Resize(new Size(, 150)));
                    draw.Mutate(o => o.Resize(new Size(imageInfo.Width, imageInfo.Height)));
                    imageToDraw.Mutate(o => o.Resize(new Size(imageInfo.Width, imageInfo.Height)));

                    outputImage.Mutate(o => o
                        .DrawImage(imageToDraw, new Point(0, 0), 0.5f) // draw the first one top left
                        .DrawImage(draw, new Point(0, 0), 1f) // draw the second next to it                    
                );
                    outputImage.Save(output, encoder);                    
                    output.Position = 0;

                    var blob = _dbContext.Blobs.FirstOrDefault(x => x.Uri == imageData.BlobUri);
                    var blobContainerClient = _blobServiceClient.GetBlobContainerClient("inventoryblobs");
                    var blobClient = blobContainerClient.GetBlobClient(blob.BlobName);
                    var response = blobClient.Upload(
                    output,
                    new BlobHttpHeaders
                    {
                        ContentType = blob.ContentType,
                    });
                    blob.BlobSize = output.Length.ToString();
                    blob.Uri = blobClient.Uri.ToString();

                    await _dbContext.SaveChangesAsync();
                }

                

                return true;
            }
            catch 
            {
                throw;
            }

        }
        private static IImageEncoder GetEncoder(string extension)
        {
            IImageEncoder encoder = null;

            extension = extension.Replace(".", "");

            var isSupported = Regex.IsMatch(extension, "gif|png|jpe?g", RegexOptions.IgnoreCase);

            if (isSupported)
            {
                switch (extension)
                {
                    case "png":
                        encoder = new PngEncoder();
                        break;
                    case "jpg":
                        encoder = new JpegEncoder();
                        break;
                    case "jpeg":
                        encoder = new JpegEncoder();
                        break;
                    case "gif":
                        encoder = new GifEncoder();
                        break;
                    default:
                        break;
                }
            }

            return encoder;
        }

        private static IImageDecoder GetDecoder(string extension)
        {
            IImageDecoder decoder = null;

            extension = extension.Replace(".", "");

            var isSupported = Regex.IsMatch(extension, "gif|png|jpe?g", RegexOptions.IgnoreCase);

            if (isSupported)
            {
                switch (extension)
                {
                    case "png":
                        decoder = new PngDecoder();
                        break;
                    case "jpg":
                        decoder = new JpegDecoder();
                        break;
                    case "jpeg":
                        decoder = new JpegDecoder();
                        break;
                    case "gif":
                        decoder = new GifDecoder();
                        break;
                    default:
                        break;
                }
            }

            return decoder;
        }
    }
}
