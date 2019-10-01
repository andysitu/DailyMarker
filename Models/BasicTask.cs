using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMarker.Models
{
    public class BasicTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

        public virtual ICollection<TaskDateTask> TaskDateTasks { get; set; }
    }
}
