using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskApplication.Models
{
    internal class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public int Balance { get; set; }
    }
}
