using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class StudentMeeting
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        public long MeetingId { get; set; }
    }
}