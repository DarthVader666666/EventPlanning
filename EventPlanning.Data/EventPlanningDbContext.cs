using EventPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Data
{
    public class EventPlanningDbContext:DbContext
    {
        public EventPlanningDbContext(DbContextOptions<EventPlanningDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<SubTheme> SubThemes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Participant> Participants { get; set; }
    }
}
