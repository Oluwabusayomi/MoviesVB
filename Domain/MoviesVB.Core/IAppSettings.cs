

namespace MoviesVB.Core
{
    public interface IAppSettings
    {
        string OpenMovieBaseUrl { get; }
        string OpenMovieApiKey {get; }
        string MoviesVbCollectionName {get; }
        string ConnectionString {get; }
        string DatabaseName {get; }
    }
}
