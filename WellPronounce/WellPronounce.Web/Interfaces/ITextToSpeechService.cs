using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;

namespace WellPronounce.Web.Interfaces
{
    public interface ITextToSpeechService
    {
        Task<string> CallCognitiveService(TextRequestModel textRequestModel);
    }
}
