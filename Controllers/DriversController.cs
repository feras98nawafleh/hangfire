using Microsoft.AspNetCore.Mvc;

namespace FireApp.Controllers;
using FireApp.Models;
using FireApp.Services;
using Hangfire;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private static List<Driver> drivers = new List<Driver>();
    private readonly ILogger<DriversController> _logger;

    public DriversController(ILogger<DriversController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("All")]
    public IActionResult GetAllDrivers()
    {
        return Ok(drivers);
    }

    [HttpPost]
    [Route("Add")]
    public IActionResult AddDriver(Driver driver)
    {
        if(ModelState.IsValid)
        {
            drivers.Add(driver);
            var jobID = BackgroundJob.Enqueue<IServiceManagement>(x => x.SendEmail());
            Console.WriteLine($"Added new driver with job ID: {jobID}");
            return CreatedAtAction("GetDriver", new {driver.id}, driver);
        }
        return BadRequest();
    }

    [HttpGet]
    [Route("Single/{id}")]
    public IActionResult GetDriver([FromRoute] Guid id)
    {
        var driver = drivers.FirstOrDefault(x => x.id == id);
        if(driver == null)
            return BadRequest($"No Driver with ID: {id}");
        return Ok(driver);
    }
    [HttpDelete]
    [Route("Delete/{id}")]
    [Obsolete] // indicates that this member function will be removed in some future version without breaking compiled code that uses that member.
    public IActionResult DeleteDriver(Guid id) 
    {
        var driver = drivers.FirstOrDefault(x => x.id == id);
        if(driver == null)
            return BadRequest($"No Driver with ID: {id}");
        driver.Status = 0;        
        // Background job is meant to execute once, 
        // either by placing it in the queue and executing immediately or by delaying the job to be executed at specific time.
        var jobID = BackgroundJob.Enqueue<IServiceManagement>(x => x.UpdateDatabase()); 

        // Recurring job is meant to trigger in certain intervals 
        // i.e. hourly, daily, thus you supply a cron expression.
        RecurringJob.AddOrUpdate<IServiceManagement>(x => x.UpdateDatabase(), Cron.Minutely);

        Console.WriteLine($"Deleted driver with ID: {id}, Job ID: {jobID}");
        return Ok(driver);
    }
}
