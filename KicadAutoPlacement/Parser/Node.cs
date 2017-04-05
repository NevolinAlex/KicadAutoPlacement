using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Node
    {
        public string Text { get; set; }
        public List<Node> Nodes;
        public Node prevNode{ get; set; }
        public Node(Node prevNode)
        {
            this.prevNode = prevNode;
            Nodes = new List<Node>();
            Text = "";
        }
    }
}
