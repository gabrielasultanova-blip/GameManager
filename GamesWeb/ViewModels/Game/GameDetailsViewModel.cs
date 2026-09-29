namespace GamesWeb.ViewModels.Game
{
    public class GameDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public string AgeRating { get; set; }
        public string Developer { get; set; }

        public List<string> Characters { get; set; } = new();
    }
}
