namespace PlaylistES.Models
{
    public class ESDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string VideoCollectionName { get; set; } = null!;
        public string PlaylistCollectionName { get; set; } = null!;

    }
}
