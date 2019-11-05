using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMarker.Models
{
    public class DailyTask
    {
        public int Id { get; set; }
        public string name { get; set; }

        public int TableTaskId { get; set; }
        public TableTask TableTask { get; set; }

        public List<DailyTask_TaskDate> DailyTask_TaskDates { get; set; }
    }
}
