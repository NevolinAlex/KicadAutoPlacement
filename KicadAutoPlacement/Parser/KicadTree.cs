using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class KicadTree
    {
        public Node Head { get; set; }
        public KicadTree()
        {
            Head = new Node(null);
        }
    }
}
