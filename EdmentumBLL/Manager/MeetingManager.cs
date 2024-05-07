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
        private readonly StudentMeetingManager _studentMeetingManager;
        private readonly TutorMeetingManager _tutorMeetingManager;
        public MeetingManager(EdmentumContext context, HttpClient httpClient, StudentMeetingManager studentmeetingManager, TutorMeetingManager tutorMeetingManager)
        {
            _httpClient = httpClient;
            _context = context;
            _studentMeetingManager = studentmeetingManager;
            _tutorMeetingManager = tutorMeetingManager;
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
                    Id = m.Id,
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
            return studentMeetings;
        }
        public void UpdateMeeting(UpdateMeeting updateReq)
        {
            try
            {
                var meetingToUpdate = _context.Meetings.FirstOrDefault(m => m.Id == updateReq.Id);
                if (meetingToUpdate != null)
                {
                    meetingToUpdate.Subject = updateReq.Subject;
                    meetingToUpdate.Comments = updateReq.Comments;
                    _context.SaveChanges();
                }

                if (updateReq.Tutors.Count != 0)
                {
                    var existingTutorMeetings = _context.TutorMeetings.Where(tm => tm.MeetingId == updateReq.Id);
                    _context.TutorMeetings.RemoveRange(existingTutorMeetings);
                    _context.SaveChanges();

                    var tutorMeetings = updateReq.Tutors.Select(tutor => new TutorMeeting
                    {
                        TutorId = tutor.TutorId,
                        MeetingId = updateReq.Id
                    }).ToList();
                    _tutorMeetingManager.AddRange(tutorMeetings);
                }

                if (updateReq.Students.Count != 0)
                {
                    var existingstudentMeetings = _context.StudentMeetings.Where(tm => tm.MeetingId == updateReq.Id);
                    _context.StudentMeetings.RemoveRange(existingstudentMeetings);
                    _context.SaveChanges();

                    var studentMeetings = updateReq.Students.Select(student => new StudentMeeting
                    {
                        StudentId = student.StudentId,
                        MeetingId = updateReq.Id
                    }).ToList();
                    _studentMeetingManager.AddRange(studentMeetings);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateMeeting: ", ex);
            }
        }
    }

}
