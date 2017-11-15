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
        private readonly Guid _userId;//private member fields start with underscore
        //"readonly" can only be set before constructor

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<NoteListItemModel> GetNotes()
        {
            using (var ctx = new ElevenNoteDbContext()) //cleans up database connections
            {
                return
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId)
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

        public bool CreateNote(NoteCreateModel model)
        {
            using (var ctx = new ElevenNoteDbContext())
            //any time we work with db, we create new context instance ("ctx")
            {
                var entity =
                    new NoteEntity
                    {
                        OwnerId = _userId,
                        Title = model.Title,
                        Content = model.Content,
                        CreatedUtc = DateTime.UtcNow
                    };
                ctx.Notes.Add(entity);
                return ctx.SaveChanges() == 1;//number of rows affected in database; should be 1
            }
        }
    }
}
