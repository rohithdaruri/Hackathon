using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WellPronounce.Web.Entities
{
    public class SpeechDetail
    {
        [Key]
        public Guid UniqueId { get; set; }
        public string InputText { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public string BlobPath { get; set; }

        public DateTime Created { get; set; }
    }
}
