using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WellPronounce.Web.ApiModels
{
    public class TextRequestModel
    {
        public string Text { get; set; }
        public string Language { get; set; }
        [JsonPropertyName("processType")]
        public string ProcessType { get; set; }
    }
}
