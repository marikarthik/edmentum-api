using EdmentumBLL.DTO;
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
                //For now hard coding it for POC
                //string authHeader = "R2N2ZzNRZ1RjNmpqZHRaVi4xNzE0NjgwNjEwMjYyOjdhMzNlY2ZhN2Q5YjBhZGU0ZWViNDEzMTk5ZTVkZjc1YmFiZjhiMGU5NDIyMWMyZTg0MzlhODQ1ZDIwOTU4NWY=";
                string authHeader = "R2N2ZzNRZ1RjNmpqZHRaVi4xNzE1MDI5Mjc5NDA3OmUyOGY4Yzc4NDVhM2RmMmEzNjQ5YmYyMjY5NzljMDFlYzNiYmYxYjNmMDUyMTAzM2UzMzk3NTI5MjZhNzRjODc=";
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
                    MeetingId = m.MeetingId,
                    MeetingLink = m.MeetingLink,
                    Tutors = _context.TutorMeetings
                        .Where(sm => sm.MeetingId == m.MeetingId)
                        .Select(sm => new TutorsList
                        {
                            TutorId = sm.TutorId,
                            TutorName = _context.Tutors
                                .Where(st => st.TutorId == sm.TutorId)
                                .Select(st => st.TutorName)
                                .FirstOrDefault()
                        }).ToList(),
                    Students = _context.StudentMeetings
                        .Where(sm => sm.MeetingId == m.MeetingId)
                        .Select(sm => new StudentsList
                        {
                            StudentId = sm.StudentId,
                            StudentName = _context.Students
                                .Where(st => st.StudentId == sm.StudentId)
                                .Select(st => st.StudentName)
                                .FirstOrDefault()
                        }).ToList(),
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
