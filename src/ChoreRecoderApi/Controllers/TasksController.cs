using System;
using System.Collections.Generic;
using System.Linq;
using ChoreRecorderApi.Model;
using ChoreRecorderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChoreRecorderApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        public ITaskService TaskService { get; set; }

        public TasksController(ITaskService taskService)
        {
            this.TaskService = taskService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<ChoreRecorderApi.Model.Task>> Get()
        {
            var userId = this.User.Identity.Name;
            return this.Ok(this.TaskService.GetAllTasksByUserId(userId));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post(Task task)
        {
            task.OwnerId = this.User.Identity.Name;
            this.TaskService.Create(task);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, Task task)
        {
            
            this.TaskService.Update(id, task);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}