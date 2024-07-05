namespace EventPlanning.Data.Entities
{
    public class Theme
    {
        public int ThemeId { get; set; }
        public string? ThemeName { get; set; }
        public SubTheme[]? SubThemes { get; set; }
    }
}
