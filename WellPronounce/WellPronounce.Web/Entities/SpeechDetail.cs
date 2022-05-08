using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WellPronounce.Web.Entities
{
    public class SpeechDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }      
        public Guid UniqueId { get; set; }
        public string InputText { get; set; }
        public string Language { get; set; }
        public string ProcessType { get; set; }
        public string BlobPath { get; set; }
        public DateTime Created { get; set; }
    }
}
