﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSW.Consulting.Application.Achievement.Queries.GetAchievementList;
using SSW.Consulting.Application.Achievement.Commands.AddAchievement;
using SSW.Consulting.WebAPI.Settings;

namespace SSW.Consulting.WebAPI.Controllers
{
    public class AchievementController : BaseController
    {
        private readonly IWWWRedirectSettings _redirectSettings;

        public AchievementController(IWWWRedirectSettings redirectSettings)
        {
            _redirectSettings = redirectSettings;
        }

        [HttpGet]
        public async Task<ActionResult<AchievementListViewModel>> List()
        {
            return Ok(await Mediator.Send(new GetAchievementListQuery()));
        }

        [HttpPost]
        public async Task<ActionResult<AddAchievementResult>> Add([FromQuery] string achievementCode)
        {
            return Ok(await Mediator.Send(new AddAchievementCommand { Code = achievementCode }));
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult TechQuiz([FromQuery]string user)
        {
            string url = string.Concat(_redirectSettings.TechQuizUrl, user);
            return Redirect(url);
        }
    }
}