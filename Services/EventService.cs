using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories;
using System;
using System.Collections.Generic;

namespace Services
{
    public class EventService : BaseService<Event, EventRepository>
    {
        public EventService(EventRepository repository) : base(repository)
        {
        }

        public bool IsStartDateTimeLessThanEndDateTime(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime < endDateTime)
            {
                return true;
            }
            return false;
        }
    }
}
