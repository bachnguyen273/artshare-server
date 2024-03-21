namespace artshare_server.WebApp.ViewModels
{
    public class GenreViewModel
    {
        public string Name { get; set; }
    }

    public class CreateGenreViewModel : GenreViewModel
    {
        public int GenreId { get; set; }
    }
}
