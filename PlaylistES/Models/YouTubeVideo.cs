using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PlaylistES.Models
{
    public class YouTubeVideo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public string Video_id { get; set; }

        [JsonPropertyName("Name")]
        public string VideoTitle { get; set; }
        public string Thumbnail { get; set; }
        public string PlaylistId { get; set; }
    }
}
