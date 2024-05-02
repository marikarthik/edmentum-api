using Newtonsoft.Json;

namespace EdmentumDAL.ModelClass
{
    public class Config
    {
        [JsonProperty("enableChat")]
        public bool EnableChat { get; set; }

        [JsonProperty("enableRecording")]
        public bool EnableRecording { get; set; }

        [JsonProperty("enableFilePlayer")]
        public bool EnableFilePlayer { get; set; }

        [JsonProperty("enableQuiz")]
        public bool EnableQuiz { get; set; }

        [JsonProperty("enablePoll")]
        public bool EnablePoll { get; set; }

        [JsonProperty("enableClassroomInvitation")]
        public bool EnableClassroomInvitation { get; set; }

        [JsonProperty("recordingFileTypes")]
        public List<string> RecordingFileTypes { get; set; }

    }
}