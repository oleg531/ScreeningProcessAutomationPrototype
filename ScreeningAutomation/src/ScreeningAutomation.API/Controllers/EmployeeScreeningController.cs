﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScreeningAutomation.API.Controllers
{
    using Services;

    [Route("api/[controller]")]
    public class EmployeeScreeningController : Controller
    {
        private readonly IScreeningStatusMonitoringService _screeningStatusMonitoringService;
        public EmployeeScreeningController(
            IScreeningStatusMonitoringService screeningStatusMonitoringService
            )
        {
            _screeningStatusMonitoringService = screeningStatusMonitoringService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _screeningStatusMonitoringService.GetEmployeeScreenings());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet()]
        [Route("CheckScreenings/{email}")]
        public async Task<IActionResult> CheckScreenings(string email)
        {
            await _screeningStatusMonitoringService.CheckScreeningsStatus(email);
            return Ok();
        }
    }
}
