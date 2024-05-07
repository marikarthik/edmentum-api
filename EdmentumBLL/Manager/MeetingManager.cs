using EdmentumBLL.DTO;
using EdmentumBLL.Utils;
using EdmentumDAL;
using EdmentumDAL.ModelClass;
using Newtonsoft.Json;
using System.Text;

namespace EdmentumBLL.Manager
{
    public class MeetingManager
    {
        private readonly EdmentumContext _context;
        private readonly HttpClient _httpClient;
        public MeetingManager(EdmentumContext context, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _context = context;
        }
        public async Task<HttpResponseMessage> CreateMeetingAsync(HiLinkMeetingRequest request)
        {
            try
            {
                string jsonRequest = JsonConvert.SerializeObject(request);
                string authHeader = BasicAuthorizationHelper.GenerateAuthorization(Constants.AccessKey, Constants.SecretKey);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
                return await _httpClient.PostAsync("https://vcaas.hilink.co/api/v2/meeting-center/meetings", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating a meeting: " + ex.Message);
            }
        }

        public void AddMeeting(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
        }

        public IEnumerable<MeetingDTO> GetAllMeetings()
        {
            var studentMeetings = _context.Meetings
                .Select(m => new MeetingDTO
                {
                    Subject = m.Subject,
                    Title = m.Title,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    MeetingId = m.HiLinkMeetingId,
                    MeetingLink = m.MeetingLink,
                    Tutors = _context.TutorMeetings
                        .Where(sm => sm.MeetingId == m.Id)
                        .Select(sm => new TutorsList
                        {
                            TutorId = sm.TutorId,
                            TutorName = _context.Tutors
                                .Where(st => st.Id == sm.TutorId)
                                .Select(st => st.TutorName)
                                .FirstOrDefault()
                        }).ToList(),
                    Students = _context.StudentMeetings
                        .Where(sm => sm.MeetingId == m.Id)
                        .Select(sm => new StudentsList
                        {
                            StudentId = sm.StudentId,
                            StudentName = _context.Students
                                .Where(st => st.Id == sm.StudentId)
                                .Select(st => st.StudentName)
                                .FirstOrDefault()
                        }).ToList(),
                    Status = m.Status,
                    Comments = m.Comments
                }).ToList();

            return studentMeetings;
        }

        public JoinTokenResponse CreateJoinTokenAsync(JoinTokenDTO request)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var meeting = _context.Meetings.FirstOrDefault(m => m.Id == request.MeetingId);
                if (meeting == null)
                {
                    throw new Exception("Meeting not found");
                }

                parameters.Add("meetingUid", meeting.HiLinkMeetingId.ToString());

                if (request.Role.ToLower() == "student")
                {
                    var student = _context.Students.FirstOrDefault(s => s.Id == request.JoineeId);
                    if (student == null)
                    {
                        throw new Exception("Joinee not found");
                    }

                    parameters.Add("externalRole", "ATTENDEE");
                    parameters.Add("externalId", request.JoineeId.ToString());
                    parameters.Add("externalName", student.StudentName);
                }
                else if (request.Role.ToLower() == "tutor")
                {
                    var tutor = _context.Tutors.FirstOrDefault(t => t.Id == request.JoineeId);
                    if (tutor == null)
                    {
                        throw new Exception("Joinee not found");
                    }

                    parameters.Add("externalRole", "HOST");
                    parameters.Add("externalId", request.JoineeId.ToString());
                    parameters.Add("externalName", tutor.TutorName);
                }

                var token = JoinMeetingTokenHelper.GenerateJoinToken(parameters);
                JoinTokenResponse joinTokenResponse = new JoinTokenResponse();
                joinTokenResponse.Token = token;
                joinTokenResponse.MeetingId = meeting.HiLinkMeetingId;
                joinTokenResponse.MeetingUrl = meeting.MeetingLink;
                return joinTokenResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating join token: " + ex.Message);
            }
        }
    }
}

