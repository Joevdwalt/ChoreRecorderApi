using System;
using System.Collections.Generic;
using ChoreRecorderApi.Model;
using ChoreRecorderApi.Helpers;
using ChoreRecorderApi.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChoreRecorderApi.Services
{
    public interface IUserProfileService
    {
        System.Threading.Tasks.Task<List<UserProfile>> GetAll();

        System.Threading.Tasks.Task<UserProfile> Create(UserProfile userProfile);

        System.Threading.Tasks.Task Update(string id, UserProfile userProfile);
    }

    public class UserProfileService : ServiceBase, IUserProfileService
    {
        public UserProfileService(IOptions<AppSettings> appSettings) : base(appSettings)
        {
        }

        public System.Threading.Tasks.Task<List<UserProfile>> GetAll()
        {
            var userProfileCollection = this.Db.GetCollection<UserProfile>("UserProfiles");
            return userProfileCollection.AsQueryable().ToListAsync();
        }

        public async System.Threading.Tasks.Task<UserProfile> Create(UserProfile userProfile)
        {
            userProfile.Id = Guid.NewGuid().ToString();
            
            var taskCollection = this.Db.GetCollection<UserProfile>("UserProfiles");

            await taskCollection.InsertOneAsync(userProfile);
            return userProfile;
        }

        public async System.Threading.Tasks.Task Update(string id, UserProfile userProfile)
        {
            var userProfileCollection = this.Db.GetCollection<UserProfile>("UserProfiles");
            var result = await userProfileCollection.ReplaceOneAsync(t => t.Id == id, userProfile);
        }

    }
}