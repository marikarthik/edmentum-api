using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class MeetingStartEnd
    {
        [Key]
        public int Id { get; set; }
        public string meetingUid { get; set; }
        public string meetingExternalId { get; set; }
        public string timestamp { get; set; }
        public int meetingStatus { get; set; }
        public int meetingEndType { get; set; }
    }
}
