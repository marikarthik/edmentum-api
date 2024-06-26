﻿// <auto-generated />
using EdmentumDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EdmentumDAL.Migrations
{
    [DbContext(typeof(EdmentumContext))]
    [Migration("20240506144045_Edm1")]
    partial class Edm1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EdmentumDAL.ModelClass.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("EndTime")
                        .HasColumnType("bigint");

                    b.Property<long>("HiLinkMeetingId")
                        .HasColumnType("bigint");

                    b.Property<string>("MeetingLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("StartTime")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.StudentMeeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentMeetings");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.Tutor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TutorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tutors");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.TutorMeeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<int>("TutorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("TutorId");

                    b.ToTable("TutorMeetings");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.StudentMeeting", b =>
                {
                    b.HasOne("EdmentumDAL.ModelClass.Meeting", "Meeting")
                        .WithMany("StudentMeetings")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EdmentumDAL.ModelClass.Student", "Student")
                        .WithMany("StudentMeetings")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.TutorMeeting", b =>
                {
                    b.HasOne("EdmentumDAL.ModelClass.Meeting", "Meeting")
                        .WithMany("TutorMeetings")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EdmentumDAL.ModelClass.Tutor", "Tutor")
                        .WithMany("TutorMeetings")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.Meeting", b =>
                {
                    b.Navigation("StudentMeetings");

                    b.Navigation("TutorMeetings");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.Student", b =>
                {
                    b.Navigation("StudentMeetings");
                });

            modelBuilder.Entity("EdmentumDAL.ModelClass.Tutor", b =>
                {
                    b.Navigation("TutorMeetings");
                });
#pragma warning restore 612, 618
        }
    }
}
