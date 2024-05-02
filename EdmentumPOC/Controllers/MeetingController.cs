//using EdmentumBLL.Manager;
//using EdmentumDAL.ModelClass;
//using Microsoft.AspNetCore.Mvc;

//namespace EdmentumPOC.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MeetingController : ControllerBase
//    {
//        private readonly MeetingManager _meetingService;

//        public MeetingController(MeetingManager meetingService)
//        {
//            _meetingService = meetingService;   
//        }
//        [HttpPost]
//        public async Task<IActionResult> CreateMeeting(MeetingRequest request)
//        {
//            try
//            {
//                var response = await _meetingService.CreateMeetingAsync(request);

//                if (response.IsSuccessStatusCode)
//                {
//                    string responseBody = await response.Content.ReadAsStringAsync();
//                    return Ok(responseBody);
//                }
//                else
//                {
//                    string errorMessage = $"Error creating meeting. Status code: {response.StatusCode}";
//                    Console.WriteLine(errorMessage);
//                    return StatusCode((int)response.StatusCode, errorMessage);
//                }
//            }
//            catch (Exception ex)
//            {
//                string errorMessage = $"Error creating meeting: {ex.Message}";
//                Console.WriteLine(errorMessage);
//                return StatusCode(500, errorMessage);
//            }
//        }
//    }
//}


using EdmentumBLL.Manager;
using EdmentumDAL.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EdmentumPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingManager _meetingManager;

        public MeetingController(MeetingManager meetingManager)
        {
            _meetingManager = meetingManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingRequest request)
        {
            try
            {
                // Create the meeting via an external API
                var response = await _meetingManager.CreateMeetingAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // If meeting creation is successful, deserialize the response body to get meeting details
                    var responseBody = await response.Content.ReadAsStringAsync();
                    dynamic meetingDetails = JsonConvert.DeserializeObject(responseBody);

                    // Extract meeting details
                    long meetingId = meetingDetails.meetingId;
                    string meetingUrl = meetingDetails.meetingUrl;

                    // Add the meeting details to the database
                    var meeting = new Meeting
                    {
                        MeetingId = meetingId,
                        MeetingLink = meetingUrl
                    };
                    _meetingManager.AddMeeting(meeting);

                    // Return a success response
                    return Ok("Meeting added successfully.");
                }
                else
                {
                    // If meeting creation fails, return an error response
                    var errorMessage = $"Error creating meeting. Status code: {response.StatusCode}";
                    Console.WriteLine(errorMessage);
                    return StatusCode((int)response.StatusCode, errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error response
                var errorMessage = $"Error creating meeting: {ex.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }
    }
}
