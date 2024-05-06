using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string StudentName { get; set; }

        // Navigation properties
        public virtual ICollection<StudentMeeting> StudentMeetings { get; set; }
    }
}
