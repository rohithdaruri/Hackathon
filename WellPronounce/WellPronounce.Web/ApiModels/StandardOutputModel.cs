using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellPronounce.Web.ApiModels
{
    public class StandardOutputModel
    {
        public string Path { get; set; }
        public string Phonetics { get; set; }
        public string UniqueId { get; set; }
    }
}
