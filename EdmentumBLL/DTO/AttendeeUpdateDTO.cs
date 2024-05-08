namespace EdmentumBLL.DTO
{
    public class AttendeeUpdateDTO
    {
        public int AttendeeId { get; set; }
        public long JoiningTime { get; set; }
        public int MeetingId { get; set; }
        public string Role { get; set; }
    }
}
