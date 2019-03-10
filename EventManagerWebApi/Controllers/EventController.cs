using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using EventManager.ViewModels.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace EventManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        #region Constructors and fields
        private readonly EventService eventService;
        private readonly UserService userService;
        private readonly SecurityService securityService;
        private readonly ModelStateWrapper modelStateWrapper;

        public EventController(EventService eventService, UserService userService, SecurityService securityService)
        {
            this.modelStateWrapper = new ModelStateWrapper(ModelState);
            this.eventService = eventService;
            this.eventService.ValidationDictionary = modelStateWrapper;
            this.userService = userService;
            this.userService.ValidationDictionary = modelStateWrapper;
            this.securityService = securityService;
        }
        #endregion

        [HttpPost("Create")]
        public ActionResult<EventViewModel> Create(EventViewModel createAnEventViewModel)
        {
            if (!eventService.PreValidate())
            {
                return BadRequest(this.modelStateWrapper.ModelStateDictionary);
            }

            Event eventDb = eventService.GetAll(x => x.Name == createAnEventViewModel.Name).FirstOrDefault();
            if (eventDb != null)
            {
                eventService.AddValidationError("eventName", "An event with this name already exists!");
                return BadRequest(modelStateWrapper.ModelStateDictionary);
            }

            if (!eventService.IsStartDateTimeLessThanEndDateTime(createAnEventViewModel.StartDateTime, createAnEventViewModel.EndDateTime))
            {
                eventService.AddValidationError("", "Start Date Time cannot be after or equal to the end date time!");
                return BadRequest(modelStateWrapper.ModelStateDictionary);
            }

            User currentUser = userService.GetAll(x => x.Id == securityService.GetUserId(HttpContext)).FirstOrDefault();
            Event @event = new Event
            {
                Name = createAnEventViewModel.Name,
                Location = createAnEventViewModel.Location,
                StartDateTime = createAnEventViewModel.StartDateTime,
                EndDateTime = createAnEventViewModel.EndDateTime
            };

            currentUser.Events.Add(@event);
            bool isSaved = userService.Save(currentUser);
            if (!isSaved)
            {
                return BadRequest();
            }
            createAnEventViewModel.Id = @event.Id;
            return Ok(createAnEventViewModel);
        }

        [HttpGet("GetMyEvents")]
        public ActionResult<EventViewModel> GetMyEvents()
        {
            User currentUser = userService.GetAll(x => x.Id == securityService.GetUserId(HttpContext)).FirstOrDefault();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();
            currentUser.Events.ForEach(x => eventViewModels.Add(new EventViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                StartDateTime = x.StartDateTime,
                EndDateTime = x.EndDateTime
            }));
            return Ok(eventViewModels);
        }

        [HttpPut("Edit")]
        public ActionResult<EventViewModel> Edit(EventViewModel eventViewModel)
        {
            if (!eventService.PreValidate())
            {
                return BadRequest(this.modelStateWrapper.ModelStateDictionary);
            }

            User currentUser = userService.GetAll(x => x.Id == securityService.GetUserId(HttpContext)).FirstOrDefault();
            Event chosenEvent = currentUser.Events.FirstOrDefault(x => x.Id == eventViewModel.Id && securityService.GetUserId(HttpContext) == x.UserId);
            if (chosenEvent == null)
            {
                return BadRequest("Event with this id not found");
            }
            chosenEvent.Name = eventViewModel.Name;
            chosenEvent.Location = eventViewModel.Location;
            chosenEvent.StartDateTime = eventViewModel.StartDateTime;
            chosenEvent.EndDateTime = eventViewModel.EndDateTime;
            if (!eventService.IsStartDateTimeLessThanEndDateTime(eventViewModel.StartDateTime, eventViewModel.EndDateTime))
            {
                eventService.AddValidationError("", "Start Date Time cannot be after or equal to the end date time!");
                return BadRequest(this.modelStateWrapper.ModelStateDictionary);
            }
            bool isSaved = userService.Save(currentUser);

            if (!isSaved)
            {
                return BadRequest();
            }
            return Ok(eventViewModel);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            User currentUserDb = userService.GetAll(x => x.Id == securityService.GetUserId(HttpContext)).FirstOrDefault();
            Event chosenEvent = currentUserDb.Events.FirstOrDefault(x => x.Id == id);
            bool isSaved = false;
            if (chosenEvent != null)
            {
                isSaved = eventService.Delete(id);
            }
            if (!isSaved)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("GetAllEvents")]
        public ActionResult<EventViewModel> GetAllEvents()
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
            return Ok(eventViewModels);
        }

    }
}