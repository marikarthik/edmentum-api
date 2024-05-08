namespace EdmentumBLL.DTO
{
    public class UserJoinLeaveDTO
    {
        public string meetingUid { get; set; }
        public string meetingExternalId { get; set; }
        public string userRole { get; set; }
        public string externalId { get; set; }
        public string externalName { get; set; }
        public string userStatus { get; set; }
        public string timestamp { get; set; }
    }
    public class MeetingStartEndDTO
    {
        public string meetingUid { get; set; }
        public string meetingExternalId { get; set; }
        public string timestamp { get; set; }
        public int meetingStatus { get; set; }
        public int meetingEndType { get; set; }
    }
}
