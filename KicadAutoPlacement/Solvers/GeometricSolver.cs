using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement.Solvers
{
    public static class GeometricSolver
    {
        public static Point GetRandomPointInRange(double width, double height)
        {
            var rnd = new Random();
            return new Point(rnd.NextDouble()*width, rnd.NextDouble()*height);
        }
    }
}
