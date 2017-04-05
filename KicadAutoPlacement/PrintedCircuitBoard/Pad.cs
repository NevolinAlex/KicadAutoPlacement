using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Pad
    {
        public Module Module { get; set; }
        public Position Position { get; set; }
        public Edge Net { get; set; }

        public Pad(Position position, Module module, Edge net)
        {
            Position = position;
            Module = module;
            Net = net;
        }
    }
}
