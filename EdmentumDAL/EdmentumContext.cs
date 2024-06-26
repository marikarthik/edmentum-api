﻿using EdmentumDAL.ModelClass;
using Microsoft.EntityFrameworkCore;

namespace EdmentumDAL
{
    public class EdmentumContext: DbContext
    {
        public DbSet<Student> Students { get; set; }    
        public DbSet<Tutor> Tutors {  get; set; }   
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<StudentMeeting> StudentMeetings { get; set; }
        public DbSet<TutorMeeting> TutorMeetings { get; set; }
        public DbSet<UserJoinLeave> UserJoinLeave { get; set; }
        public DbSet<MeetingStartEnd> MeetingStartEnd { get; set; }
        public EdmentumContext(DbContextOptions<EdmentumContext> options) : base(options)
        {
        }
        public EdmentumContext()
        {
        }
    }
}
