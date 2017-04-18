using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Module
    {
        public string Name { get; set; }
        public List<Pad> Pads;
        public Pair LeftUpperBorder { get; set; }
        public Pair RightLowerBorder { get; set; }
        public Pair Position { get; set; }
        public double Rotate { get; set; }
        public string Path { get; set; }
        public Module(string name)
        {
            Name = name;
            Position = new Pair(0, 0);
            Rotate = 0;
            Pads = new List<Pad>();
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
