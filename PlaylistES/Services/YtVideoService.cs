using PlaylistES.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PlaylistES.Services
{

    public class YtVideoService
    {
        private readonly IMongoCollection<YouTubeVideo> _VideoCollection;


        public YtVideoService(
            IOptions<ESDatabaseSettings> ESplaylistDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ESplaylistDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ESplaylistDatabaseSettings.Value.DatabaseName);

            _VideoCollection = mongoDatabase.GetCollection<YouTubeVideo>(
                ESplaylistDatabaseSettings.Value.VideoCollectionName);
        }

        public async Task<List<YouTubeVideo>> GetAsync() =>
            await _VideoCollection.Find(_ => true).ToListAsync();

        public async Task<YouTubeVideo> GetOneAsync(string id) =>
            await _VideoCollection.Find(x => x.PlaylistId == id).FirstOrDefaultAsync();

        public async Task<List<YouTubeVideo>> GetAsync(string id) =>
            await _VideoCollection.Find(x => x.PlaylistId == id).ToListAsync();

        public async Task CreateAsync(YouTubeVideo newVideo) =>
            await _VideoCollection.InsertOneAsync(newVideo);

        public async Task UpdateAsync(string id, YouTubeVideo updatedVideo) =>
            await _VideoCollection.ReplaceOneAsync(x => x.id == id, updatedVideo);

        public async Task RemoveAsync(string id) =>
            await _VideoCollection.DeleteOneAsync(x => x.id == id);
    }
}
