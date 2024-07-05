namespace EventPlanning.Server.Models
{
    public class EventModel
    {
        public string? Title { get; set; }
        public bool? DressCode { get; set; } = false;
        public string? Theme { get; set; }
        public string[]? SubThemes { get; set; }
        public string? Participants { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }
    }
}
