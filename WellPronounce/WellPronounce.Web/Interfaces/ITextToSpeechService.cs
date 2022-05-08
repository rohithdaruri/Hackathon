using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.Entities;

namespace WellPronounce.Web.Interfaces
{
    public interface ITextToSpeechService
    {
        Task<StandardOutputModel> StandardProcessSaveTextToSpeechData(TextRequestModel textRequestModel);
        Task<List<SpeechDetail>> GetRecords();
    }
}
