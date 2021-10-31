using MongoDB.Bson;
using MongoDB.Driver;
using MoviesVB.Core;
using MoviesVB.Core.Movies.Models;
using MoviesVB.Data.Repositories.Constants;

namespace MoviesVB.Data
{
    internal class MovieDbContext
    {
        private readonly MongoClient DbClient;
        private IMongoCollection<MovieDocument> _movies;
        private readonly IAppSettings _settings;


        public MovieDbContext(IAppSettings settings)
        {
            _settings = settings;
            DbClient = new MongoClient(settings.ConnectionString);
        }


        public IMongoCollection<MovieDocument> Movies
        {
            get
            {
                return _movies ?? (_movies = GetCollection<MovieDocument>(DatabaseObjectConstants.MOVIES_COLLECTION));
            }
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var database = GetDatabase();

            var settings = new MongoCollectionSettings { GuidRepresentation = GuidRepresentation.Standard };

            return database.GetCollection<T>(collectionName, settings);
        }

        public IMongoDatabase GetDatabase()
        {
            return DbClient.GetDatabase(_settings.DatabaseName);
        }
    }
}
