using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PlaylistES.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [JsonPropertyName("Name")]
        public string username { get; set; }

        public string password { get; set; }

        public string Profilepicture { get; set; }

        public ICollection<string> Playlists { get; set; }
        
    }
}
