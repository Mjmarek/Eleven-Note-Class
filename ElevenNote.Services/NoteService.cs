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
        //use true or false to determine whether or not we were able to update note
        {
            using (var ctx = new ElevenNoteDbContext())
            //any time we work with db, we create new context instance ("ctx")
            //cleans up after itself so we get those connections back
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

        public NoteDetailModel GetNoteById(int id)
        {
            NoteEntity entity;

            using (var ctx = new ElevenNoteDbContext())
            {
                entity = GetNoteById(ctx, id);
                             
            }//when "using" block is closed, connection to db is also closed

            if (entity == null) return new NoteDetailModel();
            //"SingleOrDefault" means if it can't find note with selected id & usedId,
            //it will return null by default.
            //To avoid null error, we need this line of code to return model instead.

            return
                new NoteDetailModel
                {
                    NoteId = entity.NoteId,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
        }

        public bool UpdateNote(NoteEditModel model)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity = GetNoteById(ctx, model.NoteId);
                    
                if (entity == null) return false;

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTime.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }
        
        //"private" means only accessible to metods within this class/limited scope
        private NoteEntity GetNoteById(ElevenNoteDbContext context, int noteId)
        {
            return
                context
                    .Notes
                    .SingleOrDefault(e => e.NoteId == noteId && e.OwnerId == _userId);
        }           //will only return note with selected id user requesting it also created it

        public bool DeleteNote(int noteId)//don't need to pass in a model, just the id
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity = GetNoteById(ctx, noteId);                   

                if (entity == null) return false;

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
