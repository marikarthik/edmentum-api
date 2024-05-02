using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        public long MeetingId { get; set; }
        public string MeetingLink  { get; set; }
    }
}
