namespace MyMovieAPI
{
    public class MyMovieAPIDTO
    {
        public string imdb_id { get; set; }
        public string imdb_url { get; set; }
        public string plot { get; set; }
        public string plot_simple { get; set; }
        public string rated { get; set; }
        public string rating { get; set; }
        public string rating_count { get; set; }
        public string rating_date { get; set; }
        public string runtime { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public int year { get; set; }
        public string[] actors { get; set; }
        public string[] also_know_as { get; set; }
        public string[] country { get; set; }
        public string[] directors { get; set; }
        public string[] episodes { get; set; }
        public string[] film_locations { get; set; }
        public string[] writers { get; set; }
        public string[] language { get; set; }
        public string[] genres { get; set; }
    }
}
