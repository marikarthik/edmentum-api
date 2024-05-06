using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class TutorMeeting
    {
        [Key]
        public int Id { get; set; }
        public int TutorId { get; set; }
        public long MeetingId { get; set; }
        public Meeting Meeting { get; set; } 
        public Tutor Tutor { get; set; } 
    }
}
