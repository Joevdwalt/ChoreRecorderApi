using ChoreRecorderApi.Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChoreRecorderApi.Services
{
    public class ServiceBase
    {
        public MongoClient Client {get; private set;}

        public IMongoDatabase Db { get; set; }
        public AppSettings AppSettings { get; set; }
        public ServiceBase(IOptions<AppSettings> appSettings)
        {
            this.Client = new MongoClient(appSettings.Value.MongoConnectionString);
            this.Db = this.Client.GetDatabase(appSettings.Value.MongoDatabase);
        }
    }
}