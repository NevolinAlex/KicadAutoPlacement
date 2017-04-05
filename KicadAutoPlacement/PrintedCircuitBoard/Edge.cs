using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Edge
    {
        public string Name { get; set; }
        public Pad Pad1 { get; set; }
        public Pad Pad2 { get; set; }
        public Edge(Pad pad1, Pad pad2)
        {
            Pad1 = pad1;
            Pad2 = pad2;
        }
    }
}
