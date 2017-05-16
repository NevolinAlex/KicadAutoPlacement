using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KicadAutoPlacement.GenAlgorithm;
namespace KicadAutoPlacement
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string fileName = "";
            dialog.Filter = "PCB files |*.kicad_pcb;";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
            }
            //KicadParser parser = new KicadParser("C:\\Users\\disap\\Desktop\\Diplom\\PrintedCircuitBoards\\Kicad_Projects\\kicad\\kicad.kicad_pcb");
            KicadParser parser = new KicadParser(fileName);
            PrintedCircuitBoard pcb = parser.MakePcBoardFromTree();

            //Console.WriteLine(pcb.GetIntersectionsNumber()); 

            Chromosome.LeftUpperPoint = new Point(20, 20);// левый верхний угол рабочего пространства
            Chromosome.WorkspaceHeight = 60; // высота рабочего пространства в котором генерируются новые особи
            Chromosome.WorkspaceWidth = 80;//ширина рабочего пространства в котором генерируются новые особи
            GeneticAlgorithm.PoolSize = 100; // размер пула
            GeneticAlgorithm.MaxChromosomeAge = 2; // максимальный возраст хромосомы
            GeneticAlgorithm.SelectionCount = 80; //число хромосом переживающих селекцию

            //pcb.Modules[16].Lock(); // фиксируем элемент u2 в центре
            pcb.Modules[16].LockXCoordinate(); //фиксируем по X
            //pcb.Modules[16].LockYCoordinate(); // фиксируем по Y



            Chromosome.ExamplePrintedCircuitBoard = pcb; // пример платы по которой будут генерироваться новые особи

            GeneticAlgorithm genAlgorithm = new GeneticAlgorithm();
            genAlgorithm.Start();


            Chromosome bestChromosome = genAlgorithm.GetBestChromosome();
            parser.UpdateTreeFromPcb(bestChromosome.PrintCircuitBoard);
            parser.WriteFile("out3.kicad_pcb", parser.Tree.Head);


        }
    }
}
