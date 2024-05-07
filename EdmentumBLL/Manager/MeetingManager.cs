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
                string accessKey = "Gcvg3QgTc6jjdtZV";
                string secretKey = "90f0dddf621a4619ba6c5a56adcedb1b";
                string jsonRequest = JsonConvert.SerializeObject(request);
                string authHeader = BasicAuthorizationHelper.GenerateAuthorization(accessKey, secretKey);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
                return await _httpClient.PostAsync("https://vcaas.hilink.co/api/v2/meeting-center/meetings", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void AddMeeting(Meeting meeting)
        {
            // Add the meeting details to the database
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
        }

        //public Meeting GetMeetingById(long meetingId)
        //{
        //    // Retrieve the meeting from the database by its ID
        //    return _context.Meetings.FirstOrDefault(m => m.MeetingId == meetingId);
        //}

        //public IEnumerable<Meeting> GetAllMeetings()
        //{
        //    // Retrieve all meetings from the database
        //    return _context.Meetings.ToList();
        //}

        public IEnumerable<MeetingDTO> GetAllMeetings()
        {
            var studentMeetings = _context.Meetings
                .Select(m => new MeetingDTO
                {
                    //Id = m.Id,
                    Subject = m.Subject,
                    Title = m.Title,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    //TutorId = m.TutorId,
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

            // Retrieve tutor names separately
            //foreach (var studentMeeting in studentMeetings)
            //{
            //    var tutor = _context.Tutors.FirstOrDefault(t => t.TutorId == studentMeeting.TutorId);
            //    studentMeeting.TutorName = tutor != null ? tutor.TutorName : "Bommannan R";
            //}

            return studentMeetings;
        }

    }

}
