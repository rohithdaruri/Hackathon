using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WellPronounce.Web.ApiModels
{
    public class CustomTextRequestModel
    {
        public string Text { get; set; }
        public string Language { get; set; }
        [JsonPropertyName("processType")]
        public string ProcessType { get; set; }
        [JsonPropertyName("audioFile")]
        public byte[] AudioFile { get; set; }

    }
}
