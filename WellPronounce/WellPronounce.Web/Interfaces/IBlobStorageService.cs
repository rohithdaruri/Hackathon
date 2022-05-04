using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellPronounce.Web.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType);
    }
}
