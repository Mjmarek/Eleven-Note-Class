﻿using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Web.Controllers
{
    [Authorize]//visitor to website needs to be logged in to access content below
    public class NotesController : Controller
    {
        //all methods in the controller now have access to NoteService CreateNoteService()
        //refactored, moved this code out of each individual method, and placed it here
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new NoteService(userId);
            return svc;
        }

        public ActionResult Index()
        {
            var model = CreateNoteService().GetNotes();
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new NoteCreateModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!CreateNoteService().CreateNote(model))
            {
                ModelState.AddModelError("", "Unable to create note");
                return View(model);
            }

            TempData["SaveResult"] = "Your note was created";

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var model = CreateNoteService().GetNoteById(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var detailModel = CreateNoteService().GetNoteById(id);
            var editModel =
                new NoteEditModel
                {
                    NoteId = detailModel.NoteId,
                    Title = detailModel.Title,
                    Content = detailModel.Content
                };

            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEditModel model)
        {
            if(model.NoteId != id)
            {
                ModelState.AddModelError("", "Nice try!");
                model.NoteId = id;
                return View(model);
            }

            if (!ModelState.IsValid) return View(model);

            if (!CreateNoteService().UpdateNote(model))
            {
                ModelState.AddModelError("", "Unable to update note");
                return View(model);
            }

            TempData["SaveResult"] = "Your note was saved";
            //TempData works like Session, but it gets cleaned up after it is read

            return RedirectToAction("Index");
        }

        [ActionName("Delete")]
        public ActionResult DeleteGet(int id)
        {
            var model = CreateNoteService().GetNoteById(id);
            return View(model);
        }
        //need to differentiate between these 2 functions, so we use "ActionName" attribute
        //and then change the name of the actual functions
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            CreateNoteService().DeleteNote(id);

            TempData["SaveResult"] = "Your note was deleted";

            return RedirectToAction("Index");
        }
    }
}