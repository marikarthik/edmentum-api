using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdmentumDAL.ModelClass
{
    public class StudentMeeting
    {
        [Key]
        public int Id { get; set; }
        // Foreign key relationship
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        // Foreign key relationship
        public int MeetingId { get; set; }
        [ForeignKey("MeetingId")]
        public virtual Meeting Meeting { get; set; }
        public long JoiningTime { get; set; }
        [DefaultValue("false")]
        public bool MeetingStatus { get; set; }
    }
}
