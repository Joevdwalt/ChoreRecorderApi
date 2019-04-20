using System;
using System.Collections.Generic;
using ChoreRecorderApi.Model;
using ChoreRecorderApi.Helpers;
using Microsoft.Extensions.Options;
using System.Linq;

using DateExtentionMethods;
using MongoDB.Driver;

namespace ChoreRecorderApi.Services
{
    public interface ITaskTemplateService
    {
        System.Threading.Tasks.Task<List<TaskTemplate>> Get();

        System.Threading.Tasks.Task<TaskTemplate> Create(TaskTemplate taskTemplate);

        void Update(string id, TaskTemplate taskTemplate);

        System.Threading.Tasks.Task AddTasksForDayForUser(string userid);
    }
    public class TaskTemplateService : ServiceBase, ITaskTemplateService
    {
        ITaskService TaskService = null;


        public TaskTemplateService(IOptions<AppSettings> appSettings, ITaskService taskService) : base(appSettings)
        {
            this.TaskService = taskService;
        }

        public System.Threading.Tasks.Task<List<TaskTemplate>> Get()
        {
            var taskCollection = this.Db.GetCollection<TaskTemplate>("TaskTemplates");
            return taskCollection.AsQueryable().ToListAsync();

        }

        public async System.Threading.Tasks.Task<TaskTemplate> Create(TaskTemplate taskTemplate)
        {
            taskTemplate.Id = Guid.NewGuid().ToString();
            var taskCollection = this.Db.GetCollection<TaskTemplate>("TaskTemplates");

            await taskCollection.InsertOneAsync(taskTemplate);
            return taskTemplate;
        }

        public void Update(string id, TaskTemplate taskTemplate)
        {
            var taskCollection = this.Db.GetCollection<TaskTemplate>("TaskTemplates");

            var result = taskCollection.ReplaceOne(t => t.Id == id, taskTemplate);

        }

        public async System.Threading.Tasks.Task AddTasksForDayForUser(string userId)
        {
            var tasksForUser = await this.TaskService.GetAllTasksByUserIdAndDueDate(userId, DateTime.Now.StartOfDay(), DateTime.Now.EndOfDay());
            var taskTempaltes = await this.Get();
            // if tasks to not exist create them 

            var tasksToCreate = new List<Task>();
            foreach (var template in taskTempaltes)
            {
                if (!tasksForUser.Any(t => t.Name == template.Name))
                {
                    var taskToCreate = new Task()
                    {
                        Name = template.Name,
                        DueDate = DateTime.Now,
                        Done = false,
                        OwnerId = userId,
                        Points = template.Points
                    };

                    tasksToCreate.Add(taskToCreate);
                }
            }

            foreach (var createTask in tasksToCreate)
            {
                this.TaskService.Create(createTask);
            }
        }
    }
}