using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    public class Point: IEquatable<Point>
    {
        //characters of module or pad
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Point() { }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
        public override string ToString()
        {
            return X.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) +
                " " +
                Y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }
    }
}
