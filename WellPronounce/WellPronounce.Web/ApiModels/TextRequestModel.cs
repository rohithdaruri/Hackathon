using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellPronounce.Web.ApiModels
{
    public class TextRequestModel
    {
        public string Text { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
    }
}
