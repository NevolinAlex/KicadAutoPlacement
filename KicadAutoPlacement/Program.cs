using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KicadAutoPlacement.GenAlgorithm;
namespace KicadAutoPlacement
{
    class Program
    {
        static void Main(string[] args)
        {
            KicadParser parser = new KicadParser("C:\\Users\\disap\\Desktop\\Diplom\\PrintedCircuitBoards\\Kicad_Projects\\kicad\\kicad.kicad_pcb");
            PrintedCircuitBoard pcb = parser.MakePcBoardFromTree();
            Chromosome.ExamplePrintedCircuitBoard = pcb;
            Chromosome.LeftUpperPoint = new Point(50,50);
            Chromosome.WorkspaceHeight = 100;
            Chromosome.WorkspaceWidth = 100;
            GeneticAlgorithm.PoolSize = 100;
            GeneticAlgorithm.MaxChromosomeAge = 5;
            GeneticAlgorithm.SelectionCount = 60;
            GeneticAlgorithm genAlgorithm = new GeneticAlgorithm();
            genAlgorithm.Start();
            Chromosome bestChromosome = genAlgorithm.GetBestChromosome();
            parser.UpdateTreeFromPcb(bestChromosome.PrintCircuitBoard);
            parser.WriteFile("newPcb.kicad_pcb", parser.Tree.Head);
            //Chromosome.RotateModule(pcb2.Modules[1], 90);
            //Console.WriteLine(pcb2.GetIntersectionsNumber());
            //Console.WriteLine(pcb.GetIntersectionsNumber());
            // Console.WriteLine(PrintedCircuitBoard.AreModulesIntersect(pcb.Modules[1], pcb.Modules[4]));
            //pcb.LeadToCorrectForm(new List<Module>() {pcb.Modules[1]});

        }
    }
}
