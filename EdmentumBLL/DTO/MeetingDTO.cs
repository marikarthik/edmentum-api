using EdmentumDAL.ModelClass;
using System.ComponentModel.DataAnnotations;

namespace EdmentumBLL.DTO
{
    public class MeetingDTO
    {
        public string Subject { get; set; }
        public string Title { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public long MeetingId { get; set; }
        public string MeetingLink { get; set; }
        public List<TutorsList> Tutors { get; set; }
        public List<StudentsList> Students { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }
    public class TutorsList
    {
        public int TutorId { get; set; }
        public string TutorName { get; set; }
    }
    public class StudentsList
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
    }
}



