using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Pad
    {
        public Pair Position { get; set; }
        public Edge Net { get; set; }
        public Module Module { get; set; }
        public int Number { get; set; }

        public Pad(Pair position, Edge net)
        {
            Position = position;
            Net = net;
        }
        public Pad() {
            Position = new Pair(0, 0);
        }
        public override string ToString()
        {
            return Module.ToString() + ": Pad " + Number.ToString();
        }
    }
}
