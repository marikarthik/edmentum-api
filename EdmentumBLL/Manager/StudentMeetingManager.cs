using EdmentumBLL.DTO;
using EdmentumDAL.ModelClass;
using EdmentumDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmentumBLL.Manager
{
    public class StudentMeetingManager
    {
        private readonly EdmentumContext _context;

        public StudentMeetingManager(EdmentumContext context)
        {
            _context = context;
        }

        public void AddStudentMeeting(StudentMeeting studentMeeting)
        {
            _context.StudentMeetings.Add(studentMeeting);
            _context.SaveChanges();
        }
        public void AddRange(IEnumerable<StudentMeeting> studentMeetings)
        {
            _context.StudentMeetings.AddRange(studentMeetings);
            _context.SaveChanges(); // Assuming you want to save changes immediately
        }

    }
}
