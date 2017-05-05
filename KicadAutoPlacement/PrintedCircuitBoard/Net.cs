using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    public class Net
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public Pad Pad1 { get; set; }
        public Pad Pad2 { get; set; }
        public List<Pad> Pads;
        public Net(Pad Pad1, Pad Pad2)
        {
            this.Pad1 = Pad1;
            this.Pad2 = Pad2;
        }
        public Net(string Name, int Number)
        {
            Pads = new List<Pad>();
            this.Name = Name;
            this.Number = Number;
        }
    }
}
