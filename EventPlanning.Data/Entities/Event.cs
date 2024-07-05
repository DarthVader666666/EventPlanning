namespace EventPlanning.Data.Entities
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public bool? DressCode { get; set; } = false;
        public DateTime? Date { get; set; }
        public int? AmountOfVacantPlaces { get; set; }
        public int? ThemeId { get; set; }
        public int? LocationId { get; set; }
        public Location? Location { get; set; }
        public Theme? Theme { get; set; }
    }
}
