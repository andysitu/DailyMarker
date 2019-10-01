using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DailyMarker.Models
{
    public class TaskDate
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TDate { get; set; }

        public virtual ICollection<TaskDateTask> TaskDateTasks { get; set; }
    }
}