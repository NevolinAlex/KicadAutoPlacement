using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Program
    {
        static void Main(string[] args)
        {
            KicadParser parser = new KicadParser("kicad.kicad_pcb");
            PCB pcb = new PCB(parser.Tree);
            pcb.Modules[0].Rotate = 0;
            pcb.Modules[0].Position.X -= 100;
            pcb.Modules[0].Position.Y -= 100;
            parser.WriteFromPCB(pcb);
            parser.WriteFile("newPcb.kicad_pcb", parser.Tree.Head);
        }
    }
}
