using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteEditModel
    //technically could use inehritance to get these properties,
    //but since they are being used for different things,
    //it is better not to use inheritance in this case
    {
        [Required]
        public int NoteId { get; set; }

        [Required]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Content { get; set; }

        public bool IsStarred { get; set; }

        //system will specify modified time, so we don't need to include its property
    }
}
