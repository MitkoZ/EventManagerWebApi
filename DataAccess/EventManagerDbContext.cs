using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EventManagerDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Event> Events { get; set; }

        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
        {

        }
    }
}
