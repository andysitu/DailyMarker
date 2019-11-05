using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using DailyMarker.Models;

namespace DailyMarker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<UserAccount>()
            //    .HasOne(u => u.TableTask)
            //    .WithOne(tt => tt.UserAccount)
            //    .HasForeignKey<TableTask>(t => t.UserAccount);
            //    modelBuilder.Entity<DailyTask_TaskDate>()
            //        .HasKey(dt_td => new { dt_td.DailyTaskId, dt_td.TaskDateId });

            //    modelBuilder.Entity<DailyTask_TaskDate>()
            //        .HasOne(dt_td => dt_td.DailyTask)
            //        .WithMany(dt => dt.DailyTask_TaskDates)
            //        .HasForeignKey(dt_td => dt_td.DailyTaskId);

            //    modelBuilder.Entity<DailyTask_TaskDate>()
            //        .HasOne(dt_td => dt_td.TaskDate)
            //        .WithMany(td => td.DailyTask_TaskDates)
            //        .HasForeignKey(dt_td => dt_td.TaskDateId);
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<TableTask> TableTasks { get; set; }
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<DailyTask_TaskDate> DailyTask_TaskDates { get; set; }
        public DbSet<TaskDate> TaskDates { get; set; }
    }
}
