﻿using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Grahita.pages
{
    public class BlobUploader
    {
        //azure blob storage setting
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=grahitafix;AccountKey=wxRhr0fP3BGdU0HjXfDKRrtjcs53oMJ9klqKcxeiyAnvsHjqwFrO0hCRbac+wIKgy9/ne6bgQ5/N+AStB4hkxQ==;EndpointSuffix=core.windows.net";
        private const string containerName = "fileupload";
        //
        public static async Task<string> Main(string imagePath)
        {
            // Upload image to Azure Storage Blob
            string blobName = Path.GetFileName(imagePath);
            string imageUrl = await UploadImageAsync(blobName, imagePath);

            Console.WriteLine($"Image uploaded. URL: {imageUrl}");
            return imageUrl;
        }

        static async Task<string> UploadImageAsync(string blobName, string imagePath)
        {
            // Create a BlobServiceClient
            var blobServiceClient = new BlobServiceClient(connectionString);

            // Get a reference to a container
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient(blobName);

            // Upload the image to the blob
            using (var fs = File.OpenRead(imagePath))
            {
                await blobClient.UploadAsync(fs, true);
                fs.Close();
            }

            // Get the URL of the uploaded blob
            return blobClient.Uri.ToString();
        }
    }
}
