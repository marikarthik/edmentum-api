using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class Tutor
    {
        [Key]
        public int Id { get; set; }
        public string TutorName { get; set; }

        // Navigation properties
        public virtual ICollection<TutorMeeting> TutorMeetings { get; set; }
    }
}
