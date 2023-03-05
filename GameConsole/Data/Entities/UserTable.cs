using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Data.Entities
{
    class UserTable
    {
        [Key] public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
