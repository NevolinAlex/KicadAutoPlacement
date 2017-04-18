using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    class Pair
    {
        //characters of module or pad
        public double X { get; set; }
        public double Y { get; set; }
        public Pair(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Pair() { }
        public override string ToString()
        {
            return X.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) +
                " " +
                Y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
