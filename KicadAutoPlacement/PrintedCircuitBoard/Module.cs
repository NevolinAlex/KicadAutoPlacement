using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Module
    {
        public string Name { get; set; }
        public List<Pad> Pads;
        public Position Position { get; set; }

        public Module(string name, Position position)
        {
            Name = name;
            Position = position;
        }

    }
}
