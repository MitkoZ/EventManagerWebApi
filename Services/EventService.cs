using DataAccess.Models;
using Repositories;
using Services.Interfaces;
using System;

namespace Services
{
    public class EventService : BaseService<Event, EventRepository>
    {
        public EventService(IValidationDictionary validationDictionary, EventRepository repository) : base(validationDictionary, repository)
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
