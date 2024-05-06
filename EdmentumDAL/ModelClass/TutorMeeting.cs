using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdmentumDAL.ModelClass
{
    public class TutorMeeting
    {
        [Key]
        public int Id { get; set; }
        // Foreign key relationship
        public int TutorId { get; set; }
        [ForeignKey("TutorId")]
        public virtual Tutor Tutor { get; set; }
        // Foreign key relationship
        public int MeetingId { get; set; }
        [ForeignKey("MeetingId")]
        public virtual Meeting Meeting { get; set; }
    }
}
