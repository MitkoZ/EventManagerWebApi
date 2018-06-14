using DataAccess.Models;
using EventManager.Helpers;
using EventManager.ViewModels.Event;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManager.Controllers
{
    public class EventController : Controller
    {
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
            EventService eventService = new EventService();
            if (!ModelState.IsValid)
            {
                return View(createAnEventViewModel);
            }

            UserRepository userRepository = new UserRepository();
            EventRepository eventRepository = new EventRepository();
            Event eventDb = eventRepository.GetAll(x => x.Name == createAnEventViewModel.Name).FirstOrDefault();
            if (eventDb != null)
            {
                ModelState.AddModelError("", "An event with this name already exists!");
                return View("CreateAnEvent", createAnEventViewModel);
            }

            if (!eventService.IsStartDateTimeLessThanEndDateTime(createAnEventViewModel.StartDateTime, createAnEventViewModel.EndDateTime))
            {
                ModelState.AddModelError("", "Start Date Time cannot be after or equal to the end date time!");
                return View("CreateAnEvent", createAnEventViewModel);
            }

            User currentUser = userRepository.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
            currentUser.Events.Add(new Event
            {
                Name = createAnEventViewModel.Name,
                Location = createAnEventViewModel.Location,
                StartDateTime = createAnEventViewModel.StartDateTime,
                EndDateTime = createAnEventViewModel.EndDateTime
            });
            bool isSaved = userRepository.Save(currentUser) > 0;
            if (!isSaved)
            {
                ModelState.AddModelError("", "Ooops something went wrong!");
                return View("CreateAnEvent");
            }
            TempData["SuccessMessage"] = "Event created successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult GetMyEvents()
        {
            UserRepository userRepository = new UserRepository();
            User currentUser = userRepository.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
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
            EventRepository eventRepository = new EventRepository();
            Event chosenEvent = eventRepository.GetAll(x => x.Id == id).FirstOrDefault();
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
            EventService eventService = new EventService();
            if (!ModelState.IsValid)
            {
                return View("CreateAnEvent", eventViewModel);
            }

            UserRepository userRepository = new UserRepository();
            User currentUser = userRepository.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
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
                ModelState.AddModelError("", "Start Date Time cannot be after or equal to the end date time!");
                return View("CreateAnEvent", eventViewModel);
            }
            bool isSaved = userRepository.Save(currentUser) > 0;

            if (!isSaved)
            {
                ModelState.AddModelError("", "Ooops something went wrong!");
                return View("CreateAnEvent");
            }
            TempData["SuccessMessage"] = "Event edited successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            UserRepository userRepository = new UserRepository();
            User currentUserDb = userRepository.GetAll(x => x.Id == LoginUserSession.Current.UserId).FirstOrDefault();
            Event chosenEvent = currentUserDb.Events.FirstOrDefault(x => x.Id == id);
            EventRepository eventRepository = new EventRepository();
            bool isSaved = false;
            if (chosenEvent != null)
            {
                isSaved = eventRepository.Delete(id) > 0;
            }
            if (!isSaved)
            {
                ModelState.AddModelError("", "Ooops something went wrong!");
                return RedirectToAction("Index", "Home");
            }
            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult GetAllEvents()
        {
            EventRepository eventRepository = new EventRepository();
            List<Event> eventsDb = eventRepository.GetAll();
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