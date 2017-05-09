using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    public class Pad
    {
        public Point Position { get; set; }//
        public Net Net { get; set; }
        public string Name { get; set; }
        public Module Module { get; set; }//
        public int Number { get; set; }//

        public Pad(Point position, Net net)
        {
            Position = position;
            Net = net;
        }
        public Pad() {
            Position = new Point(0, 0);
        }
        public override string ToString()
        {
            return Module.ToString() + ": Pad " + Number.ToString();
        }
    }
}
