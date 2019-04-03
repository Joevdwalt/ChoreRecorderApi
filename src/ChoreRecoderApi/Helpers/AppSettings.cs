namespace ChoreRecorderApi.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string MongoConnectionString { get; set; }
        public string MongoDatabase { get; set; }
    }
}