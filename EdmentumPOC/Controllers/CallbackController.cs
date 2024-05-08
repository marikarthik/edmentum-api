using EdmentumBLL.DTO;
using EdmentumBLL.Manager;
using EdmentumPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EdmentumPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly CallbackManager _callbackManager;
        public CallbackController(CallbackManager callbackManager)
        {
            _callbackManager = callbackManager;
        }
        [Route("userjoinleave")]
        [HttpPost]
        public IActionResult userjoinleave(UserJoinLeaveDTO request)
        {
            //string filePath = "userjoinleave.txt";
            try
            {
                //string content = JsonConvert.SerializeObject(request);
                //System.IO.File.WriteAllText(filePath, content);
                _callbackManager.AddUserJoinLeave(request);
                ReturnResponse result = new ReturnResponse();
                result.message = "Received successfully.";
                result.statuscode = 200;
                return Ok(result);
            }
            catch (Exception ex)
            {
                //System.IO.File.WriteAllText(filePath, "Exception: " + ex.Message.ToString());
                var errorMessage = $"Error: {ex.Message}";
                return StatusCode(500, errorMessage);
            }
        }
        [Route("meetingstartend")]
        [HttpPost]
        public IActionResult meetingstartend(MeetingStartEndDTO request)
        {
            //string filePath = "meetingstartend.txt";
            try
            {
                //string content = JsonConvert.SerializeObject(request);
                //System.IO.File.WriteAllText(filePath, content);
                _callbackManager.AddMeetingStartEnd(request);
                ReturnResponse result = new ReturnResponse();
                result.message = "Received successfully.";
                result.statuscode = 200;
                return Ok(result);
            }
            catch (Exception ex)
            {
                //System.IO.File.WriteAllText(filePath, "Exception: " + ex.Message.ToString());
                var errorMessage = $"Error: {ex.Message}";
                return StatusCode(500, errorMessage);
            }
        }
    }
}
