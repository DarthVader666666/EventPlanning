namespace EventPlanning.Data.Entities
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public bool? DressCode { get; set; } = false;
        public int? ThemeId { get; set; }
        public int? LocationId { get; set; }
        public int? ParticipantId { get; set; }
        public Participant? Participant { get; set; }
        public Location? Location { get; set; }
        public DateTime? Date { get; set; }
        public Theme? Theme { get; set; }
    }
}
