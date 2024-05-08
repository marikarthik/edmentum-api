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
                string authHeader = BasicAuthorizationHelper.GenerateAuthorization(Constants.AccessKey, Constants.SecretKey);
                //string authHeader = "R2N2ZzNRZ1RjNmpqZHRaVi4xNzE1MTEwNzc2ODU3OjcxNmIxODEwYzU1NzJkN2E3NDBmYzhjMWI2YWNmNWY2NmI5ZjA0NzljYmNhN2E4ZmRmNjJjYzUzN2M2OGZkZmI=";
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

        public void UpdateMeetingStatus(int meetingId, string status)
        {
            try
            {
                var meetingToUpdate = _context.Meetings.FirstOrDefault(m => m.Id == meetingId);
                if (meetingToUpdate != null)
                {
                    meetingToUpdate.Status = status;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating meeting status: ", ex);
            }
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

        public void UpdateAttendeeInfo(int attendeeId, long joiningTime, int meetingId, string role)
        {
            try
            {
                var existingStudentMeeting = _context.StudentMeetings.FirstOrDefault(sm => sm.StudentId == attendeeId && sm.MeetingId == meetingId);
                var existingTutorMeeting = _context.TutorMeetings.FirstOrDefault(tm => tm.TutorId == attendeeId && tm.MeetingId == meetingId);

                if (role.ToLower() == "student")
                {
                    if (existingStudentMeeting != null)
                    {
                        existingStudentMeeting.JoiningTime = joiningTime;
                        existingStudentMeeting.MeetingStatus = true;
                    }
                    else
                    {
                        var studentMeeting = new StudentMeeting
                        {
                            StudentId = attendeeId,
                            MeetingId = meetingId,
                            JoiningTime = joiningTime,
                            MeetingStatus = true
                        };
                        _context.StudentMeetings.Add(studentMeeting);
                    }
                }
                else if (role.ToLower() == "tutor")
                {
                    if (existingTutorMeeting != null)
                    {
                        existingTutorMeeting.JoiningTime = joiningTime;
                        existingTutorMeeting.MeetingStatus = true;
                    }
                    else
                    {
                        var tutorMeeting = new TutorMeeting
                        {
                            TutorId = attendeeId,
                            MeetingId = meetingId,
                            JoiningTime = joiningTime,
                            MeetingStatus = true
                        };
                        _context.TutorMeetings.Add(tutorMeeting);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid role provided.");
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating attendee information.", ex);
            }
        }

    }
}

