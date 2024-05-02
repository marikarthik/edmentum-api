using Newtonsoft.Json;

namespace EdmentumDAL.ModelClass
{
    public class MeetingRequest
    {
        [JsonProperty("meetingExternalId")]
        public string MeetingExternalId { get; set; }

        [JsonProperty("meetingTitle")]
        public string MeetingTitle { get; set; }

        [JsonProperty("startTime")]
        public long StartTime { get; set; }

        [JsonProperty("endTime")]
        public long EndTime { get; set; }

        [JsonProperty("countdownStartTime")]
        public int CountdownStartTime { get; set; }

        [JsonProperty("callbackUrl")]
        public string CallbackUrl { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty("meetingRegion")]
        public string MeetingRegion { get; set; }

        [JsonProperty("invitationUrl")]
        public string InvitationUrl { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonProperty("docIds")]
        public List<string> DocIds { get; set; }

        [JsonProperty("lessonPlanUuids")]
        public List<string> LessonPlanUuids { get; set; }

        [JsonProperty("quizIds")]
        public List<string> QuizIds { get; set; }
    }
}