using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KicadAutoPlacement.GenAlgorithm;
using KicadAutoPlacement.Solvers;

namespace KicadAutoPlacement
{
    class KicadParser
    {
        public KicadTree Tree;
        public readonly string DirectoryPath;
        public KicadParser(string filePath)
        {
            string fullFilePath = Path.GetFullPath(filePath);
            DirectoryPath = Path.GetDirectoryName(fullFilePath);
            using (StreamReader sr = new StreamReader(fullFilePath))
            {

                Tree = new KicadTree();
                Tree.Head = Parse(sr, Tree.Head).Nodes[0];
                Tree.Head.prevNode = null;
                
            }
        }

        private Node Parse(StreamReader sr, Node curNode)
        {
            bool flag = true;
            while (!sr.EndOfStream)
            {
                char curSymbol = (char)sr.Read();

                switch (curSymbol)
                {
                    case '(':
                        flag = true;
                        curNode.Nodes.Add(Parse(sr,new Node(curNode)));
                        break;
                    case ')':
                        flag = false;
                        return curNode;
                    case '\n':
                        break;
                    case '\r':
                        break;
                    case '"':
                        curNode.Text += curSymbol;
                        while((curSymbol = (char)sr.Read()) != '"')
                        {
                            curNode.Text += curSymbol;
                        }
                        curNode.Text += curSymbol;
                        break;
                    default:
                        if (flag)
                            curNode.Text += curSymbol;
                        break;
                }
            }
            return curNode;
        }

        public void WriteFile(string fileName, Node head)
        {
            string fileText = "";
            using (StreamWriter file = new StreamWriter(DirectoryPath+"\\"+ fileName,false))
            {
                fileText = Writing(fileText, head);
                file.WriteLine(fileText);
            }
        }
        private string Writing(string text, Node curNode)
        {
            curNode.Text = curNode.Text.Replace("hide", "");
            text += '(' + curNode.Text;
            foreach(Node node in curNode.Nodes)
            {
                text = Writing(text, node);
            }
            text += ")\n";
            return text;
        }
        public void UpdateTreeFromPcb(PrintedCircuitBoard pcb)
        {
            int count = 0;
            foreach (Node curModule in Tree.Head.Nodes.Where(x => x.Text.Contains("module")))
            {
                string rotate = ((pcb.Modules[count].Rotate == 0) ? "" : pcb.Modules[count].Rotate.ToString());
                int padCount = 0;
                if (curModule.Text.Split(' ').ToList()[1] == (pcb.Modules[count].Name))
                {
                    foreach(Node character in curModule.Nodes)
                    {

                        List<string> values = character.Text.Split(' ').ToList();
                        switch (values[0])
                        {
                            case "at":
                                character.Text =String.Format("at {0} {1}",
                                    pcb.Modules[count].Position.ToString(),
                                    rotate);
                                break;
                            case "fp_text":
                                List<string> textPos = character.Nodes[0].Text.Split(' ').ToList();
                                character.Nodes[0].Text = String.Format("at {0} {1} {2}",
                                    textPos[1],
                                    textPos[2],
                                    rotate);
                                break;
                            case "pad":
                                List<string> splitText = character.Nodes[0].Text.Split(' ').ToList();
                                character.Nodes[0].Text = String.Format("at {0} {1} {2}",
                                    splitText[1],
                                    splitText[2],
                                     rotate);
                                padCount++;
                                break;
                        }
                    }
                    
                }
                count++;
            }
        }

        public PrintedCircuitBoard MakePcBoardFromTree()
        {
            PrintedCircuitBoard pcBoard = new PrintedCircuitBoard();
            pcBoard.Modules = new List<Module>();
            pcBoard.NetList = new List<Net>();
            foreach (Node curModule in Tree.Head.Nodes.Where(x => x.Text.Contains("module")))
            {
                Point min = new Point(double.MaxValue, double.MaxValue);
                Point max = new Point(double.MinValue, double.MinValue);
                Module module = new Module(curModule.Text.Split(' ')[1]);
                foreach (Node character in curModule.Nodes)
                {
                    List<string> values = character.Text.Split(' ').ToList();
                    switch (values[0])
                    {
                        case "path":
                            module.Path = values[1];
                            break;
                        case "at":
                            List<double> position = GetCoordinates(new List<Node> { character });
                            module.Position.X = position[0];
                            module.Position.Y = position[1];
                            module.Rotate = (position.Count == 3) ? position[2] : 0;
                            break;
                        case "fp_line":
                            List<double> list = GetCoordinates(character.Nodes.Take(2).ToList());
                            min = GeometricSolver.GetMin(GeometricSolver.GetMin(min, list[0], list[1]), list[2], list[3]);
                            max = GeometricSolver.GetMax(GeometricSolver.GetMax(max, list[0], list[1]), list[2], list[3]);
                            break;
                        case "fp_circle":
                            break;
                        case "pad":
                            Pad pad = new Pad();
                            int n;
                            if (int.TryParse(values[1], out n))
                            {
                                pad.Number = int.Parse(values[1]);
                            }
                            else pad.Name = values[1];

                            List<double> pos = GetCoordinates(character.Nodes.Where(x => x.Text.Contains("at")).ToList());
                            pad.Position.X = pos[0];
                            pad.Position.Y = pos[1];
                            List<double> size = GetCoordinates(character.Nodes.Where(x => x.Text.Contains("size")).ToList());
                            List<string> curNet = character.Nodes.Where(x => x.Text.Contains("net")).SelectMany(x => x.Text.Split(' ')).ToList();
                            Net net = null;
                            if (curNet.Count != 0)
                            {
                                foreach (var edge in pcBoard.NetList)
                                {
                                    if (edge.Number == int.Parse(curNet[1]) && edge.Name == curNet[2])
                                        net = edge;
                                }
                                if (net == null)
                                {

                                    pad.Net = new Net(curNet[2], int.Parse(curNet[1]));
                                    pad.Net.Pad1 = pad;
                                    pad.Net.Pads.Add(pad);
                                    pcBoard.NetList.Add(pad.Net);
                                }
                                else
                                {
                                    pad.Net = net;
                                    pad.Net.Pad2 = pad;
                                    pad.Net.Pads.Add(pad);
                                }
                            }
                            pad.Module = module;
                            min = GeometricSolver.GetMin(min, pos[0] - size[0] / 2, pos[1] - size[1] / 2);
                            max = GeometricSolver.GetMax(max, pos[0] + size[0] / 2, pos[1] + size[1] / 2);
                            module.Pads.Add(pad);
                            break;
                    }
                }
                module.LeftUpperBound = min;
                module.RighLowerBound = max;

                var temp = module.Rotate;
                Chromosome.RotateModule(module, module.Rotate);
                module.Rotate = temp;
                pcBoard.Modules.Add(module);
            }
            return pcBoard;
        }

        private List<double> GetCoordinates(List<Node> list)
        {
            double value = 0;
            return list
                .SelectMany(x => x.Text.Split(' '))
                .Where(x => double.TryParse(x, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                .Select(x => value)
                .ToList();
        }
    }
}
