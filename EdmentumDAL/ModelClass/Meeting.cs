using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdmentumDAL.ModelClass
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string MeetingLink { get; set; }
        public long HiLinkMeetingId { get; set; }

        // Navigation properties
        public virtual ICollection<TutorMeeting> TutorMeetings { get; set; }
        public virtual ICollection<StudentMeeting> StudentMeetings { get; set; }
    }
}
