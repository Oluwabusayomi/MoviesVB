namespace MoviesVB.Core
{
    public class AppSettings : IAppSettings
    {
        public string OpenMovieBaseUrl { get; set; }
        public string OpenMovieApiKey { get; set; }
        public string MoviesVbCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }
}
