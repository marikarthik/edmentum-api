using EdmentumDAL;
using EdmentumDAL.ModelClass;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmentumBLL.Manager
{
    public class TutorMeetingManager
    {
        private readonly EdmentumContext _context;
        public TutorMeetingManager(EdmentumContext context)
        {
            _context = context;
        }
        public void AddTutorMeeting(TutorMeeting tutorMeeting)
        {
            _context.TutorMeetings.Add(tutorMeeting);
            _context.SaveChanges();
        }
        public void AddRange(IEnumerable<TutorMeeting> tutorMeetings)
        {
            _context.TutorMeetings.AddRange(tutorMeetings);
            _context.SaveChanges(); // Assuming you want to save changes immediately
        }
    }
}
