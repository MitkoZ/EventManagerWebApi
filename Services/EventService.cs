using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EventService
    {
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
