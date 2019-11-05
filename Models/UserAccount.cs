using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyMarker.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public TableTask TableTask { get; set; }
        public int TableTaskId { get; set; }
    }
}