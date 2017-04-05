using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Position
    {
        //characters of module or pad
        public double X { get; set; }
        public double Y { get; set; }
        public double Rotate { get; set; }
        public Position(double X, double Y, double rotate)
        {
            this.X = X;
            this.Y = Y;
            Rotate = rotate;
        }
    }
}
