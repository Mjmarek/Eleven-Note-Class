using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteCreateModel//the properties listed here are being provided by user
    //other properties will be provided by system (ex. NoteId, OwnerId, etc)
    {
        [Required]//make sure these attributes are consistent with properties in NoteEntity
        public string Title { get; set; }

        [MaxLength(500)]
        public string Content { get; set; }
    }
}
