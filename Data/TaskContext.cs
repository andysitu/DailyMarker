using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DailyMarker.Models;

namespace DailyMarker.Data
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
        }

        public DbSet<BasicTask> BasicTasks { get; set; }
        public DbSet<TaskDate> TaskDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDateTask>()
                .HasKey(tdt => new { tdt.BasicTaskId, tdt.TaskDateId });

            modelBuilder.Entity<TaskDateTask>()
                .HasOne(tdt => tdt.TaskDate)
                .WithMany(td => td.TaskDateTasks)
                .HasForeignKey(tdt => tdt.TaskDateId);

            modelBuilder.Entity<TaskDateTask>()
                .HasOne(tdt => tdt.BasicTask)
                .WithMany(t => t.TaskDateTasks)
                .HasForeignKey(tdt => tdt.BasicTaskId);

            
        }

        public DbSet<DailyMarker.Models.TaskDateTask> TaskDateTask { get; set; }
    }
}
