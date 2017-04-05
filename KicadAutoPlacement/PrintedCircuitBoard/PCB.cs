using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class PCB
    {
        public List<Module> Modules; // list of elements
        public List<Edge> Edges; // list of nets

        public int GetIntersectionsNumber()
        {
            // TODO: get number of the intersections
            throw new NotImplementedException();
        }

        public PCB(KicadTree tree)
        {
            // TODO: make PCB from tree
        }
        public PCB(PCB pcb)
        {
            // TODO:make a clone PCB from another PCB
        }

    }
}
