namespace EventPlanning.Server.Models
{
    public class EventIndexModel
    {
        public int? EventId { get; set; }
        public string? Title { get; set; }
        public bool? DressCode { get; set; }
        public string? ThemeName { get; set; }
        public string[]? SubThemeNames { get; set; }
        public string? Participants { get; set; }
        public string? Location { get; set; }
        public int? AmountOfVacantPlaces { get; set; }
        public DateTime? Date { get; set; }
    }
}
