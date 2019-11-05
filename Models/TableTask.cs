using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyMarker.Models
{
    public class TableTask
    {
        public int Id { get; set; }

        [ForeignKey("UserAccountId")]
        public UserAccount UserAccount { get; set; }
        public int UserAccountId { get; set; }

        public List<DailyTask> DailyTasks { get; set; }
    }
}