using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMarker.Models
{
    public class DailyTask_TaskDate
    {
        public int Id { get; set; }
        public int DailyTaskId { get; set; }
        public DailyTask DailyTask { get; set; }

        public int TaskDateId { get; set; }
        public TaskDate TaskDate { get; set; }
    }
}