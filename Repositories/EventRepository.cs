using DataAccess;
using DataAccess.Models;

namespace Repositories
{
    public class EventRepository : BaseRepository<Event>
    {
        public EventRepository(EventManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
