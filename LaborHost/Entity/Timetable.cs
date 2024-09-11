using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Entity
{

    class TimetableContext : DbContext
    {
        DbSet<Timetable> Timetables { get; set; }

        public string DbPath { get; }

        public TimetableContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "timetables.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Timetable>().OwnsOne(t => t.Schedule);
        }
    }


    public class Timetable
    {
        public int TimetableId { get; set; } // primary key
        public required string Course { get; set; }
        public required string LecturerName { get; set; }
        public required string Semester { get; set; }
        public required string StudyProgram { get; set; }
        public required Schedule Schedule { get; set; }


    }

    public class Schedule
    {
        public int ScheduleId { get; set; }  // Primary Key
        public required DayOfWeek DayWeek { get; set; }
        public required ScheduleTime TimeStart { get; set; }
        public required ScheduleTime TimeEnd { get; set; }

    }

    public class ScheduleTime
    {
        public int ScheduleTimeId { get; set; }  // Primary Key

        public required int Hour { get; set; }
        public required int Minute { get; set; }

    }
}