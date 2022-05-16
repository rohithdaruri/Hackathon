using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.AppConfig;
using WellPronounce.Web.Interfaces;

namespace WellPronounce.Web.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        string accessKey = string.Empty;

        public BlobStorageService(IConfiguration configuration)
        {
            accessKey = configuration.GetConnectionString("AccessKey");
        }

        public async Task<AzureBlobPathModel> UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {

                AzureBlobPathModel fileUrl = await UploadFileToBlobAsync(strFileName, fileData, fileMimeType);
                return fileUrl;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        private async Task<AzureBlobPathModel> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            var azureBlobPathModel = new AzureBlobPathModel();
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "texttospeechconversions";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                string fileName = strFileName + ".wav";

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = "audio/wav";
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);

                    //var appPath = Directory.GetCurrentDirectory();

                    //// provide the file download location below
                    //string downloadedFile = @$"{appPath}\BlobFile\{fileName}";
                    
                    //using (var file = File.OpenWrite(downloadedFile))
                    //{
                    //    await cloudBlockBlob.DownloadToStreamAsync(file);
                    //    azureBlobPathModel.DownloadPath = downloadedFile;
                    //}

                    azureBlobPathModel.BlobPath = cloudBlockBlob.Uri.AbsoluteUri;
                    return azureBlobPathModel;
                }
                return azureBlobPathModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
