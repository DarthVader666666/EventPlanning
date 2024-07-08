using EventPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Data
{
    public class EventPlanningDbContext:DbContext
    {
        public EventPlanningDbContext(DbContextOptions<EventPlanningDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParticipantEvent>().HasKey(x => new { x.EventId, x.ParticipantId });
            modelBuilder.Entity<UserEvent>().HasKey(x => new { x.EventId, x.UserId });
            modelBuilder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<SubTheme> SubThemes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<ParticipantEvent> ParticipantEvents { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
