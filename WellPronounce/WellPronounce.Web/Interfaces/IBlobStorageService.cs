using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;

namespace WellPronounce.Web.Interfaces
{
    public interface IBlobStorageService
    {
        Task<AzureBlobPathModel> UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType);
    }
}
