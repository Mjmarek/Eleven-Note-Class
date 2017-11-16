using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Data
{

    [Table("Note")]//defines table name; by default, class name would be the title (here, "NoteEntity")
    public class NoteEntity //C# representation of note within database
    {
        [Key]//will uniquely identify note in database; primary key; sets up identity column
        public int NoteId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string Title { get; set; }

        [MaxLength(500)]//max length will limit amount of storage established by database
        public string Content { get; set; }

        [DefaultValue(false)]
        public bool IsStarred { get; set; }

        [Required]
        public DateTime CreatedUtc { get; set; }//value type means it must have a value

        public DateTime? ModifiedUtc { get; set; }//"?" after value type means it is nullable
        //Utc is universal time; it is very important to store time consistently w/in program
    }
}