using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASKMANAGER
{
    public class DataGridItems
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; }
    }
}

