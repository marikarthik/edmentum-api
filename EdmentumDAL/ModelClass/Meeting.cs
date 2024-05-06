using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace EdmentumDAL.ModelClass
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        public string Subject {get; set; }
        public string Title { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public long MeetingId { get; set; }
        public string MeetingLink  { get; set; }

        // Navigation properties
        public ICollection<StudentMeeting> StudentMeetings { get; set; }
        public ICollection<TutorMeeting> TutorMeetings { get; set; }
    }
}
