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
    public class CallbackManager
    {
        private readonly EdmentumContext _context;

        public CallbackManager(EdmentumContext context)
        {
            _context = context;
        }
        public void AddUserJoinLeave(UserJoinLeaveDTO request)
        {
            var ObjUserJoinLeave = new UserJoinLeave
            {
                meetingUid = request.meetingUid,
                meetingExternalId = request.meetingExternalId,
                userRole = request.userRole,
                externalId = request.externalId,
                externalName = request.externalName,
                userStatus = request.userStatus,
                timestamp = request.timestamp
            };
            _context.UserJoinLeave.Add(ObjUserJoinLeave);
            _context.SaveChanges();
        }
        public void AddMeetingStartEnd(MeetingStartEndDTO request)
        {
            var ObjMeetingStartEnd = new MeetingStartEnd
            {
                meetingUid = request.meetingUid,
                meetingExternalId = request.meetingExternalId,
                timestamp = request.timestamp,
                meetingStatus = request.meetingStatus,
                meetingEndType = request.meetingEndType
            };
            _context.MeetingStartEnd.Add(ObjMeetingStartEnd);
            _context.SaveChanges();
        }
    }
}
