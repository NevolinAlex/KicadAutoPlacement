using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            this.Modules = new List<Module>();
            this.Edges = new List<Edge>();
            foreach (Node curModule in tree.Head.Nodes.Where(x => x.Text.Contains("module")))
            {
                Pair min = new Pair(double.MaxValue, double.MaxValue);
                Pair max = new Pair(double.MinValue, double.MinValue);
                Module module = new Module(curModule.Text.Split(' ')[1]);
                //var pairsXY = element.Nodes
                //    .Where(x => x.Text.Contains("fp_line"))
                //    .SelectMany(x => x.Nodes.Take(2))
                //    .SelectMany(x => x.Text.Split(' '))
                //    .Where(x => double.TryParse(x, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                //    .Select(x => value)
                //    .ToList();
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
                            min = GetMin(GetMin(min, list[0], list[1]), list[2], list[3]);
                            max = GetMax(GetMax(max, list[0], list[1]), list[2], list[3]);
                            //TODO: сделать подсчет минмакс
                            break;
                        case "fp_circle":
                            break;
                        case "pad":
                            Pad pad = new Pad();
                            pad.Number = int.Parse(values[1]);
                            List<double> pos = GetCoordinates(character.Nodes.Where(x => x.Text.Contains("at")).ToList());
                            pad.Position.X = pos[0];
                            pad.Position.Y = pos[1];
                            List<double> size = GetCoordinates(character.Nodes.Where(x => x.Text.Contains("size")).ToList());
                            List<string> curNet = character.Nodes.Where(x => x.Text.Contains("net")).SelectMany(x => x.Text.Split(' ')).ToList();
                            pad.Net = new Edge(curNet[2], int.Parse(curNet[1]));
                            pad.Net.Pad1 = pad;
                            pad.Module = module;
                            this.Edges.Add(pad.Net);
                            min = GetMin(min, pos[0] - size[0] / 2, pos[1] - size[1] / 2);
                            max = GetMax(max, pos[0] + size[0] / 2, pos[1] + size[1] / 2);
                            module.Pads.Add(pad);
                            break;
                    }
                }
                module.LeftUpperBorder = min;
                module.RightLowerBorder = max;
                this.Modules.Add(module);
            }
        }
        private Pair GetMin(Pair pair, double X, double Y)
        {
            pair.X = (X < pair.X) ? X : pair.X;
            pair.Y = (Y < pair.Y) ? Y : pair.Y;
            return pair;
        }
        private Pair GetMax(Pair pair, double X, double Y)
        {
            pair.X = (X > pair.X) ? X : pair.X;
            pair.Y = (Y > pair.Y) ? Y : pair.Y;
            return pair;
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
        public PCB(PCB pcb)
        {
            // TODO:make a clone PCB from another PCB
        }

    }
}
