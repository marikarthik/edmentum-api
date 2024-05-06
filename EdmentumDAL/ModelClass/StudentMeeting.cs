﻿using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class StudentMeeting
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public long MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public Student Student { get; set; }
    }
}