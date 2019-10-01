using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMarker.Models
{
    public class TaskDateTask
    {
        public int BasicTaskId { get; set; }
        public BasicTask BasicTask { get; set; }

        public int TaskDateId { get; set; }
        public TaskDate TaskDate { get; set; }
    }
}
