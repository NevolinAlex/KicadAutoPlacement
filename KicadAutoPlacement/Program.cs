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
            parser.WriteFile("newPcb.kicad_pcb", parser.Tree.Head);
        }
    }
}
