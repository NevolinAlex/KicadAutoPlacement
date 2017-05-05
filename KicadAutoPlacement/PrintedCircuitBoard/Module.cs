using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    public class Module
    {
        public string Name { get; set; }//
        public List<Pad> Pads;
        public Point LeftUpperBound { get; set; }//
        public Point RighLowerBound { get; set; }//
        public Point Position { get; set; }//
        private double _rotate;
        public double Rotate
        {
            get { return _rotate; }
            set { _rotate = value % 360; }
        } //

        public string Path { get; set; }//
        public Module(string name)
        {
            Name = name;
            Position = new Point(0, 0);
            Rotate = 0;
            LeftUpperBound = new Point(0,0);
            RighLowerBound = new Point(0,0);
            Pads = new List<Pad>();
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
