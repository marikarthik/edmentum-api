using EdmentumBLL.DTO;
using EdmentumBLL.Manager;
using EdmentumDAL.ModelClass;
using EdmentumPOC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EdmentumPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingManager _meetingManager;
        private readonly TutorMeetingManager _tutorMeetingManager;
        private readonly StudentMeetingManager _studentMeetingManager;

        public MeetingController(MeetingManager meetingManager, TutorMeetingManager tutormeetingManager, StudentMeetingManager studentmeetingManager)
        {
            _meetingManager = meetingManager;
            _tutorMeetingManager = tutormeetingManager;
            _studentMeetingManager = studentmeetingManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingRequestDTO request)
        {
            try
            {
                HiLinkMeetingRequest ObjmeetingRequest = ArrangeInput(request);
                // Create the meeting via an external API
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
                        HiLinkMeetingId = meetingId,
                        MeetingLink = meetingUrl,
                        Status = "Scheduled",
                        Comments = ""
                    };
                    _meetingManager.AddMeeting(meeting);

                    //Add tutor-student relationships to the database
                    var tutorMeetings = request.Tutors.Select(tutor => new TutorMeeting
                    {
                        TutorId = tutor.TutorId,
                        MeetingId = meeting.Id
                    }).ToList();
                    _tutorMeetingManager.AddRange(tutorMeetings);

                    var studentMeetings = request.Students.Select(student => new StudentMeeting
                    {
                        StudentId = student.StudentId,
                        MeetingId = meeting.Id
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
                var errorMessage = $"Error creating meeting: {ex.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }

        [HttpGet]
        public IActionResult GetAllMeetings()
        {
            try
            {
                var studentMeetings = _meetingManager.GetAllMeetings();

                return Ok(studentMeetings);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error retrieving student meetings: {ex.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }
        [HttpPost("updateMeetingStatus/{meetingId}/{status}")]
        public IActionResult UpdateMeetingStatus(int meetingId, string status)
        {
            try
            {
                ReturnResponse result = new ReturnResponse();
                _meetingManager.UpdateMeetingStatus(meetingId, status);
                result.message = "Meeting status updated successfully.";
                result.statuscode = 200;
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error updating meeting status: {ex.Message}";
                return StatusCode(500, errorMessage);
            }
        }

        private HiLinkMeetingRequest ArrangeInput(MeetingRequestDTO meetingReq)
        {
            HiLinkMeetingRequest meetingRequest = new HiLinkMeetingRequest();
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
        [Route("UpdateMeeting")]
        [HttpPost]
        public IActionResult UpdateMeeting(UpdateMeeting updateReq)
        {
            ReturnResponse result = new ReturnResponse();
            try
            {
                _meetingManager.UpdateMeeting(updateReq); 
                result.message = "Updated Successfully.";
                result.statuscode = 200;
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error retrieving meeting: {ex.Message}";
                return StatusCode(500, errorMessage);
            }
        }
        [HttpPost("create-join-token")]
        public async Task<IActionResult> CreateJoinToken(JoinTokenDTO request)
        {
            try
            {
                var result = _meetingManager.CreateJoinTokenAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error generating the token: {ex.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }
    }
}
