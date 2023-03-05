using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Data.Entities
{
    class SnakeTable
    {
        [Key] public int ID { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime DateStarted { get; set; }
    }
}
