using PlaylistES.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PlaylistES.Services
{

    public class UserService
    {
        private readonly IMongoCollection<User> _UserCollection;


        public UserService(
            IOptions<ESDatabaseSettings> ESplaylistDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ESplaylistDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ESplaylistDatabaseSettings.Value.DatabaseName);

            _UserCollection = mongoDatabase.GetCollection<User>(
                ESplaylistDatabaseSettings.Value.UserCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _UserCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetOneIDAsync(string id) =>
            await _UserCollection.Find(x => x.UserId == id).FirstOrDefaultAsync();

        public async Task<User?> GetOneAsync(string username, string password) =>
            await _UserCollection.Find(x => x.username == username && x.password == password).FirstOrDefaultAsync();

        public async Task<User?> checkUsernameAsync(string username) =>
           await _UserCollection.Find(x => x.username == username).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _UserCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _UserCollection.ReplaceOneAsync(x => x.UserId == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _UserCollection.DeleteOneAsync(x => x.UserId == id);

    }
}
