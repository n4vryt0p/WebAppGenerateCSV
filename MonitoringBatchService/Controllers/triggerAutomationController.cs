using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MonitoringBatchService.Data;
using MonitoringBatchService.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonitoringBatchService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TriggerAutomationController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public TriggerAutomationController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecords()
        {
            try
            {
               
            
                var records = _databaseService.UpdateRecordsAsyncruls(1, "OPEN");
                var recordsResponse = new
                {
                    Code = 200,
                    Result = "Success,",
                    Content = new
                    {
                        Message = "Status has been updated to 1-Open.",
                      
                    }
                };
                var response = JsonSerializer.Serialize(recordsResponse);
              await _databaseService.GetLogs(response);
                return Ok(recordsResponse); // Return 200 dengan data
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return 400 dengan pesan error
            }
        }
    }
}
