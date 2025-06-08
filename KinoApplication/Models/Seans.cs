using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoApplication.Models
{
    public class Seans
    {
        public Film Film { get; set; } = null!;
        public Sala Sala { get; set; } = null!;
        public DateTimeOffset Data { get; set; }
    }
}