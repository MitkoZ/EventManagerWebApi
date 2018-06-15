using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Models;
using EventManager.Helpers;
using EventManager.ViewModels.Event;
using Repositories;
using Services;

namespace EventManager.Controllers
{
    public class EventController : Controller
    {
        #region Constructors and fields
        private EventService eventService;
        private UserService userService;

        public EventController()
        {
            ModelStateWrapper modelStateWrapper = new ModelStateWrapper(ModelState);
            this.eventService = new EventService(modelStateWrapper, new EventRepository());
            this.userService = new UserService(modelStateWrapper, new UserRepository());
        }
        #endregion

        [HttpGet]
        public ActionResult Create()
        {
            EventViewModel createAnEventViewModel = new EventViewModel();
            DateTime currentDateTime = DateTime.Now;
            createAnEventViewModel.StartDateTime = currentDateTime;
            createAnEventViewModel.EndDateTime = currentDateTime.AddMinutes(1);
            return View("CreateAnEvent", createAnEventViewModel);
        }

        [HttpPost]
        public ActionResult Create(EventViewModel createAnEventViewModel)
        {
            if (!eventService.PreValidate())
            {
                return View("CreateAnEvent", createAnEventViewModel);
            }

            Event eventDb = eventService.GetAll(x => x.Name == createAnEventViewModel.Name).FirstOrDefault();
            if (eventDb != null)
            {
                eventService.AddValidationError("", "An event with this name already exists!");
                return View("CreateAnEvent", createAnEventViewModel);
            }

            if (!eventService.IsStartDateTimeLessThanEndDateTime(createAnEventViewModel.StartDateTime, createAnEventViewModel.EndDateTime))
            {
                eventService.AddValidationError("", "Start Date Time cannot be after or equal to the end date time!");
                return View("CreateAnEvent", createAnEventViewModel);
            }

            User currentUser = userService.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
            currentUser.Events.Add(new Event
            {
                Name = createAnEventViewModel.Name,
                Location = createAnEventViewModel.Location,
                StartDateTime = createAnEventViewModel.StartDateTime,
                EndDateTime = createAnEventViewModel.EndDateTime
            });
            bool isSaved = userService.Save(currentUser);
            if (!isSaved)
            {
                eventService.AddValidationError("", "Ooops something went wrong!");
                return View("CreateAnEvent");
            }
            TempData["SuccessMessage"] = "Event created successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult GetMyEvents()
        {
            User currentUser = userService.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();
            currentUser.Events.ForEach(x => eventViewModels.Add(new EventViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                StartDateTime = x.StartDateTime,
                EndDateTime = x.EndDateTime
            }));
            return View("GetEvents", eventViewModels);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Event chosenEvent = eventService.GetAll(x => x.Id == id).FirstOrDefault();
            if (chosenEvent == null)
            {
                TempData["ErrorMessage"] = "A event with the specified id doesn't exist";
                return RedirectToAction("GetMyEvents", "Event");
            }
            EventViewModel eventViewModel = new EventViewModel
            {
                Id = chosenEvent.Id,
                Name = chosenEvent.Name,
                Location = chosenEvent.Location,
                StartDateTime = chosenEvent.StartDateTime,
                EndDateTime = chosenEvent.EndDateTime
            };
            return View("CreateAnEvent", eventViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EventViewModel eventViewModel)
        {
            if (!eventService.PreValidate())
            {
                return View("CreateAnEvent", eventViewModel);
            }

            User currentUser = userService.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
            Event chosenEvent = currentUser.Events.FirstOrDefault(x => x.Id == eventViewModel.Id && LoginUserSession.Current.UserId == x.UserId);
            if (chosenEvent == null)
            {
                TempData["ErrorMessage"] = "Event with this id not found";
                return RedirectToAction("Index", "Home");
            }
            chosenEvent.Name = eventViewModel.Name;
            chosenEvent.Location = eventViewModel.Location;
            chosenEvent.StartDateTime = eventViewModel.StartDateTime;
            chosenEvent.EndDateTime = eventViewModel.EndDateTime;
            if (!eventService.IsStartDateTimeLessThanEndDateTime(eventViewModel.StartDateTime, eventViewModel.EndDateTime))
            {
                eventService.AddValidationError("", "Start Date Time cannot be after or equal to the end date time!");
                return View("CreateAnEvent", eventViewModel);
            }
            bool isSaved = userService.Save(currentUser);

            if (!isSaved)
            {
                eventService.AddValidationError("", "Ooops something went wrong!");
                return View("CreateAnEvent");
            }
            TempData["SuccessMessage"] = "Event edited successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            User currentUserDb = userService.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
            Event chosenEvent = currentUserDb.Events.FirstOrDefault(x => x.Id == id);
            bool isSaved = false;
            if (chosenEvent != null)
            {
                isSaved = eventService.Delete(id);
            }
            if (!isSaved)
            {
                eventService.AddValidationError("", "Ooops something went wrong!");
                return RedirectToAction("Index", "Home");
            }
            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult GetAllEvents()
        {
            List<Event> eventsDb = eventService.GetAll();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();
            eventsDb.ForEach(x => eventViewModels.Add(
                    new EventViewModel
                    {
                        Name = x.Name,
                        Location = x.Location,
                        StartDateTime = x.StartDateTime,
                        EndDateTime = x.EndDateTime
                    })
                );
            return View("GetEvents", eventViewModels);
        }

    }
}