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

            Chromosome.LeftUpperPoint = new Point(20,20);
            Chromosome.WorkspaceHeight = 50;
            Chromosome.WorkspaceWidth = 50;
            GeneticAlgorithm.PoolSize = 100;
            GeneticAlgorithm.MaxChromosomeAge = 5;
            GeneticAlgorithm.SelectionCount = 60;
            KicadParser parser = new KicadParser("C:\\Users\\disap\\Desktop\\Diplom\\PrintedCircuitBoards\\Kicad_Projects\\kicad\\kicad.kicad_pcb");
            PrintedCircuitBoard pcb = parser.MakePcBoardFromTree();
            Chromosome.ExamplePrintedCircuitBoard = pcb;
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
