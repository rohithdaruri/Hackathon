using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.Entities;

namespace WellPronounce.Web.Interfaces
{
    public interface ITextToSpeechRepository
    {
        Task<StandardOutputModel> GetDetailByName(StandardTextRequestModel standardTextRequestModel);
        Task<StandardOutputModel> StandardProcessSaveTextToSpeechData(string blobPath, StandardTextRequestModel standardTextRequestModel);
        Task<List<SpeechDetail>> GetRecords();
    }
}
