using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        public IEnumerable<NoteListItemModel> GetNotes()
        {
            using (var ctx = new ElevenNoteDbContext()) //cleans up database connections
            {
                return
                    ctx
                        .Notes
                        .Select(//transforms NoteEntity into another shape so controller can understand it
                            e => //"e" is individual NoteEntity from Db
                                new NoteListItemModel
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc,
                                    ModifiedUtc = e.ModifiedUtc
                                })
                        .ToArray();//load all data to array

            }
        }
    }
}
