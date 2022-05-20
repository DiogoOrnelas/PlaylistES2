using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PlaylistES.Models
{
    public class Playlists
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlaylistId { get; set; }

        public string Creator_id { get; set; }

        [JsonPropertyName("Name")]
        public string PlaylistName { get; set; }

        public string PlaylistDescription { get; set; }

        public string PlaylistImage { get; set; }

        public ICollection<string> Videos { get; set; }
    }
}
