using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.Web.Controllers.WebAPI
//WebAPI meant for programs talkng to each other, code talking to the server
//not meant to interact with user directly;
//that's why we're returning bools instead of views (like in MVC)
{
    [Authorize]
    [RoutePrefix("api/Notes")]
    public class NotesController : ApiController
    {
        private bool SetStarState(int noteId, bool newState)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);

            var detail = service.GetNoteById(noteId);

            var updatedNote =
                new NoteEditModel
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content,
                    IsStarred = newState
                };

            return service.UpdateNote(updatedNote);//returns success or failure
        }

        [Route("{id}/Star")]//the star is a resource attached to the note
        public bool Put(int id)//"put" means create something
        {
            return SetStarState(id, true);
        }

        [Route("{id}/Star")]
        public bool Delete(int id)//"delete" means remove something
        {
            return SetStarState(id, false);
        }
    }
}
