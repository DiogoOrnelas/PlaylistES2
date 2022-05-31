using PlaylistES.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PlaylistES.Services
{

    public class PlaylistsService
    {

        private readonly IMongoCollection<Playlists> _PlaylistCollection;

        public PlaylistsService(
            IOptions<ESDatabaseSettings> ESplaylistDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ESplaylistDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ESplaylistDatabaseSettings.Value.DatabaseName);

            _PlaylistCollection = mongoDatabase.GetCollection<Playlists>(
                ESplaylistDatabaseSettings.Value.PlaylistCollectionName);
        }
        public async Task<List<Playlists>> GetAsync() =>
            await _PlaylistCollection.Find(_ => true).ToListAsync();

        public async Task<List<Playlists>> GetFromUserAsync(string id) =>
            await _PlaylistCollection.Find(x => x.Creator_id == id).ToListAsync();

        public async Task<Playlists?> GetOneAsync(string id) =>
            await _PlaylistCollection.Find(x => x.PlaylistId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Playlists newPlaylist) =>
            await _PlaylistCollection.InsertOneAsync(newPlaylist);

        
        public async Task UpdateAsync(string id, Playlists updatedPlaylist) =>
            await _PlaylistCollection.ReplaceOneAsync(x => x.PlaylistId == id, updatedPlaylist);
        
        public async Task RemoveAsync(string id) =>
            await _PlaylistCollection.DeleteOneAsync(x => x.PlaylistId == id);
    }
}
