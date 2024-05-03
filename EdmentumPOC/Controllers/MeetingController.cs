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

namespace EdmentumPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingManager _meetingManager;
        private readonly StudentMeetingManager _studentMeetingManager;

        public MeetingController(MeetingManager meetingManager, StudentMeetingManager studentmeetingManager)
        {
            _meetingManager = meetingManager;
            _studentMeetingManager = studentmeetingManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingReq request)
        {
            try
            {
                MeetingRequest ObjmeetingRequest = new MeetingRequest();
                ObjmeetingRequest = ArrangeInput(request);
                //Create the meeting via an external API
                var response = await _meetingManager.CreateMeetingAsync(ObjmeetingRequest);
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
                        Subject = request.Subject,
                        Title = request.Title,
                        StartTime = request.StartTime,
                        EndTime = request.EndTime,
                        TutorId = request.Tutor,
                        MeetingId = meetingId,
                        MeetingLink = meetingUrl
                    };
                    _meetingManager.AddMeeting(meeting);

                    // Assuming request.Students is a collection of Student objects
                    var studentMeetings = request.Students.Select(student => new StudentMeeting
                    {
                        StudentId = student.StudentId,
                        MeetingId = meetingId
                    }).ToList();
                    _studentMeetingManager.AddRange(studentMeetings);
                    // Return a success response with meeting details
                    return Ok(new { MeetingId = meetingId, MeetingUrl = meetingUrl });
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

        [HttpGet("{meetingId}")]
        public IActionResult GetMeeting(long meetingId)
        {
            try
            {
                // Get the meeting details 
                var meeting = _meetingManager.GetMeetingById(meetingId);

                if (meeting != null)
                {
                    // Return the meeting details
                    return Ok(meeting);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error retrieving meeting: {ex.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }

        [HttpGet]
        public IActionResult GetAllMeetings()
        {
            try
            {
                // Retrieve all meetings from the MeetingManager
                var meetings = _meetingManager.GetAllMeetings();

                // Return the meetings as a response
                return Ok(meetings);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error retrieving meetings: {ex.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }
        private MeetingRequest ArrangeInput(MeetingReq meetingReq)
        {
            MeetingRequest meetingRequest = new MeetingRequest();

            meetingRequest.MeetingTitle = meetingReq.Title;

            meetingRequest.CountdownStartTime = 5;
            meetingRequest.CallbackUrl = "<https://www.hilink.co/>";
            meetingRequest.RedirectUrl = "<https://www.hilink.co/>";
            meetingRequest.InvitationUrl = "<https://www.hilink.co/>";
            meetingRequest.MeetingRegion = "us-east-2";
            meetingRequest.MeetingExternalId = "1000001";
            meetingRequest.Config = new Config();
            meetingRequest.Config.EnableChat = true;
            meetingRequest.Config.EnableRecording = true;
            meetingRequest.Config.EnableFilePlayer = true;
            meetingRequest.Config.EnableQuiz = true;
            meetingRequest.Config.EnablePoll = true;
            meetingRequest.Config.EnableClassroomInvitation = true;
            meetingRequest.Config.RecordingFileTypes = ["mp4", "mp3"];
            //meetingRequest.Config.RecordingFileTypes = ["mp4, mp3"];

            meetingRequest.DocIds = [];
            meetingRequest.LessonPlanUuids = [];
            meetingRequest.QuizIds = [];
            // subject
            meetingRequest.StartTime=meetingReq.StartTime;
            meetingRequest.EndTime = meetingReq.EndTime;
            //tutor
            //students
            return meetingRequest;
        }
    }
}
