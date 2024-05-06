using EdmentumDAL.ModelClass;
using System.ComponentModel.DataAnnotations;

namespace EdmentumBLL.DTO
{
    public class StudentMeetingDTO
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public int TutorId { get; set; }
        public string TutorName { get; set; }
        public long MeetingId { get; set; }
        public string MeetingLink { get; set; }
        public List<StudentList> Students { get; set; }
        //public Meeting Meeting { get; set; }
        //public Student Student { get; set; }
        //public Tutor Tutor { get; set; }    
    }
    public class StudentList
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
    }
}



