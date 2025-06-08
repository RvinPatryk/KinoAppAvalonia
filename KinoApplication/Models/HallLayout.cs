using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoApplication.Models
{
    public class HallLayout
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<RowLayout> Rows { get; set; } = new();
    }
}
