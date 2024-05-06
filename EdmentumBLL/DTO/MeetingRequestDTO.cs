﻿using EdmentumDAL.ModelClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmentumBLL.DTO
{
    public class MeetingRequestDTO
    {
        [JsonProperty("Subject")]
        public string Subject { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Date")]
        public DateOnly Date { get; set; }
        [JsonProperty("StartTime")]
        public long StartTime { get; set; }
        [JsonProperty("EndTime")]
        public long EndTime { get; set; }
        [JsonProperty("Tutor")]
        public List<TutorList> Tutors { get; set; }
        [JsonProperty("Students")]
        public List<StudentList> Students { get; set; }
    }
    public class TutorList
    {
        public int TutorId { get; set; }
    }
    public class StudentList
    {
        public int StudentId { get; set; }
    }
    public class HiLinkMeetingRequest
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
