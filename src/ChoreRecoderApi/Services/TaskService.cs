using System;
using System.Collections.Generic;
using ChoreRecorderApi.Model;
using ChoreRecorderApi.Helpers;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace ChoreRecorderApi.Services
{
    public interface ITaskService
    {
        IEnumerable<Task> GetAllTasksByUserId(string userId);
        Task Create(Task task);

        void Update(string id, Task task);
    }
    public class TaskService : ServiceBase, ITaskService
    {
        public TaskService(IOptions<AppSettings> appSettings) : base(appSettings)
        {
        }

        public IEnumerable<Task> GetAllTasksByUserId(string userId)
        {
            var taskCollection = this.Db.GetCollection<Task>("Tasks");
            return taskCollection.Find(t => t.OwnerId == userId)
                                 .ToEnumerable();
        }

        public Task Create(Task task)
        {
            task.Id = Guid.NewGuid().ToString();
            var taskCollection = this.Db.GetCollection<Task>("Tasks");

            taskCollection.InsertOne(task);
            return task;
        }

        public void Update(string id, Task task)
        {
            var taskCollection = this.Db.GetCollection<Task>("Tasks");

            var result = taskCollection.ReplaceOne(t=> t.Id == id, task);

        }
    }
}