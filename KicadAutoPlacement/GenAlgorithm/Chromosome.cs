using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KicadAutoPlacement.Solvers;

namespace KicadAutoPlacement.GenAlgorithm
{
    public class Chromosome
    {
        public static PrintedCircuitBoard ExamplePrintedCircuitBoard = null;
        public static double WorkspaceWidth = 50;
        public static double WorkspaceHeight = 50;
        public static Point LeftUpperPoint = new Point(0,0);
        public PrintedCircuitBoard PrintCircuitBoard;
        public int Age { get; set; }
        public int Valuation { get; private set; }// { get { return CalculateValuation(); } }
        public static int Angle = 90;

        /// <summary>
        /// Создание новой хромосомы по образцу, с случайным разположением модулей
        /// </summary>
        public Chromosome()
        {
            Age = 0;
            Valuation = int.MaxValue;
            PrintCircuitBoard = new PrintedCircuitBoard(ExamplePrintedCircuitBoard);
            foreach (var module in PrintCircuitBoard.Modules)
            {
                module.Position = GeometricSolver.GetRandomPointInRange(WorkspaceWidth, WorkspaceHeight) + LeftUpperPoint;
            }
        }

        /// <summary>
        /// Полное копирование хромосомы, включая конфигурацию платы которую она хранит
        /// </summary>
        /// <param name="chromosome"></param>
        public Chromosome(Chromosome chromosome)
        {
            PrintCircuitBoard = new PrintedCircuitBoard(chromosome.PrintCircuitBoard);
            PrintCircuitBoard.LeadToCorrectForm(PrintCircuitBoard.Modules);
            Age = chromosome.Age;
            Valuation = chromosome.Valuation;
        }

        /// <summary>
        /// Создание новой хромосомы с сылкой на конфигурацию платы без копирования
        /// </summary>
        /// <param name="pcBoard"></param>
        public Chromosome(PrintedCircuitBoard pcBoard)
        {
            Age = 0;
            Valuation = int.MaxValue;
            PrintCircuitBoard = pcBoard;
        }

        public int CalculateValuation()
        {
            Valuation = this.PrintCircuitBoard.GetIntersectionsNumber();
            return Valuation;
        }

        /// <summary>
        /// Смена позиций одного элемента из одной конфигурации в другую
        /// </summary>
        /// <param name="chromosome1"></param>
        /// <param name="chromosome2"></param>
        /// <returns>Две хромосомы получившиеся в результате скрещивания</returns>
        public static List<Chromosome> Crossing(Chromosome chromosome1, Chromosome chromosome2)
        {
            PrintedCircuitBoard printedCircuitBoard1 = new PrintedCircuitBoard(chromosome1.PrintCircuitBoard);
            PrintedCircuitBoard printedCircuitBoard2 = new PrintedCircuitBoard(chromosome2.PrintCircuitBoard);
            Random rnd = new Random();
            int elementForCross = rnd.Next(printedCircuitBoard1.Modules.Count);
            var temp = printedCircuitBoard1.Modules[elementForCross].Position;
            printedCircuitBoard1.Modules[elementForCross].Position =
                printedCircuitBoard2.Modules[elementForCross].Position;
            printedCircuitBoard2.Modules[elementForCross].Position = temp;
            printedCircuitBoard1.LeadToCorrectForm(new List<Module>() {printedCircuitBoard1.Modules[elementForCross]});
            printedCircuitBoard2.LeadToCorrectForm(new List<Module>() { printedCircuitBoard2.Modules[elementForCross] });
            return new List<Chromosome>() {new Chromosome(printedCircuitBoard1), new Chromosome(printedCircuitBoard2)};
        }

        /// <summary>
        /// Созданий двух новых хромосом на основе текущей 
        /// (два случайных модуля меняются местами, случайный модуль поворачивается на случайный градус)
        /// </summary>
        /// <returns></returns>
        public List<Chromosome> Mutate()
        {
            //todo: Поворот элемента: need Refactor
            PrintedCircuitBoard rotatedPcb = new PrintedCircuitBoard(this.PrintCircuitBoard);
            Random rnd = new Random();
            int mutateNumber = rnd.Next(rotatedPcb.Modules.Count);
            var angle = rnd.Next(4) * Angle;
            RotateModule(rotatedPcb.Modules[mutateNumber], angle);
            rotatedPcb.LeadToCorrectForm(new List<Module>() { rotatedPcb.Modules[mutateNumber]}); 

            //todo: Смена двух элементов местами: need Refactor
            PrintedCircuitBoard swappedPcb = new PrintedCircuitBoard(this.PrintCircuitBoard);
            SwapModules(swappedPcb);
            return  new List<Chromosome>() {new Chromosome(swappedPcb), new Chromosome(rotatedPcb)};
        }

        /// <summary>
        /// Поворот модуля на заданный градус
        /// </summary>
        /// <param name="module"></param>
        /// <param name="angle"></param>
        public static void RotateModule(Module module, double angle)
        {
            double radians = angle * Math.PI / 180;
            Point newCoordinate1 = GeometricSolver.RotatePoint(module.LeftUpperBound, radians);
            Point newCoordinate2 = GeometricSolver.RotatePoint(module.RighLowerBound, radians);
            module.LeftUpperBound = GeometricSolver.GetMin(newCoordinate1, newCoordinate2.X, newCoordinate2.Y);
            module.RighLowerBound = GeometricSolver.GetMax(newCoordinate2, newCoordinate1.X, newCoordinate1.Y);

            foreach (var pad in module.Pads)
            {
                pad.Position = GeometricSolver.RotatePoint(pad.Position, radians);
            }
            module.Rotate += angle;

        }

        /// <summary>
        /// Обмен позициями двух случайных элементов
        /// </summary>
        /// <param name="pcb"></param>
        public static void SwapModules(PrintedCircuitBoard pcb)
        {
            Random rnd = new Random();
            var modulesToSwap = pcb.Modules.OrderBy(x => rnd.Next()).Take(2).ToList();
            var temp = modulesToSwap[0].Position;
            modulesToSwap[0].Position = modulesToSwap[1].Position;
            modulesToSwap[1].Position = temp;
            pcb.LeadToCorrectForm(new List<Module>() {modulesToSwap[0], modulesToSwap[1]});
            //return pcb;
        }
    }
}
