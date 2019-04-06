using System;
using System.Collections.Generic;
using System.Linq;
using ChoreRecorderApi.Model;
using ChoreRecorderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DateExtentionMethods;

namespace ChoreRecorderApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TaskTemplatesController : ControllerBase
    {

        public ITaskTemplateService TaskTemplateService { get; set; }

        public TaskTemplatesController(ITaskTemplateService taskTemplateService)
        {
            this.TaskTemplateService = taskTemplateService;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<List<TaskTemplate>>> Get()
        {
            var result = await this.TaskTemplateService.Get();
            return Ok(result);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Post(TaskTemplate taskTemplate)
        {
            await this.TaskTemplateService.Create(taskTemplate);
            return Ok();
        }

        [HttpPost("addtasksforday")]
        public async System.Threading.Tasks.Task<ActionResult> AddTasksForDay()
        {

            var userid = this.User.Identity.Name;
            
            await this.TaskTemplateService.AddTasksForDayForUser(userid);
            return Ok();
        }


    }
}