using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class UserJoinLeave
    {
        [Key]
        public int Id { get; set; }
        public string meetingUid { get; set; }
        public string meetingExternalId { get; set; }
        public string userRole { get; set; }
        public string externalId { get; set; }
        public string externalName { get; set; }
        public string userStatus { get; set; }
        public string timestamp { get; set; }
    }
}
