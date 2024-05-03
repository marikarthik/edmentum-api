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
        public int TutorId { get; set; }
        public long MeetingId { get; set; }
        public string MeetingLink  { get; set; }
    }
}
