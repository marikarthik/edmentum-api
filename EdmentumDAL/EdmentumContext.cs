using EdmentumDAL.ModelClass;
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
        public EdmentumContext(DbContextOptions<EdmentumContext> options) : base(options)
        {

        }

        public EdmentumContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<StudentMeeting>()
            //    .HasOne(u => u.Meeting)
            //    .WithMany(u => u.StudentMeetings)
            //    .HasForeignKey(u => u.MeetingId);




        }
}
