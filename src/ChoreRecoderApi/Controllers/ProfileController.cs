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
    public class ProfileController : ControllerBase
    {
        public IUserProfileService UserProfileService { get; set; }

        public ProfileController(IUserProfileService userProfileService)
        {
            this.UserProfileService = userProfileService;
        }

        // GET api/values
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<IEnumerable<ChoreRecorderApi.Model.UserProfile>>> Get()
        {
            var userProfiles = await this.UserProfileService.GetAll();

            return this.Ok(userProfiles);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task Post(UserProfile  userProfile)
        {
             await this.UserProfileService.Create(userProfile);
        } 

    }
}