using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Data.Entities
{
    class TicTacToeTable
    {
        [Key] public int ID { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public DateTime DateStarted { get; set; }
    }
}
