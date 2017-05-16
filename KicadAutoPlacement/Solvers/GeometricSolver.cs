using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement.Solvers
{
    public static class GeometricSolver
    {
        public static Point GetRandomPointInRange(double width, double height, Random rnd)
        {
            return new Point(rnd.Next(0,(int)width), rnd.Next(0,(int)height));
        }

        /// <summary>
        /// Проверка на пересечение двух отрезков на плоскости
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static bool AreSegmentsIntersect(Point a1, Point a2, Point b1, Point b2)
        {
            double eps = 0.000001;
            double d = (a1.X - a2.X) * (b2.Y - b1.Y) - (a1.Y - a2.Y) * (b2.X - b1.X);
            double da = (a1.X - b1.X) * (b2.Y - b1.Y) - (a1.Y - b1.Y) * (b2.X - b1.X);
            double db = (a1.X - a2.X) * (a1.Y - b1.Y) - (a1.Y - a2.Y) * (a1.X - b1.X);

            if (Math.Abs(d) < eps)
                return false;
            else
            {
                double ta = da / d;
                double tb = db / d;
                if ((0 <= ta) && (ta <= 1.000001) && (0 <= tb) && (tb <= 1.000001))
                    return true;
                return false;
            }

        }
        /// <summary>
        /// Возвращает расстояние между двумя точками
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
        /// <summary>
        /// Поворот точки на заданый градус относительно (0,0)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static Point RotatePoint(Point point, double radians)
        {
            double newX = point.X * Math.Cos(radians) + point.Y * Math.Sin(radians);
            double newY = -point.X * Math.Sin(radians) + point.Y * Math.Cos(radians);
            return new Point(newX, newY);
        }
        public static Point GetMin(Point point, double X, double Y)
        {
            return new Point((X < point.X) ? X : point.X, (Y < point.Y) ? Y : point.Y);
        }
        public static Point GetMax(Point point, double X, double Y)
        {
            return new Point((X > point.X) ? X : point.X, (Y > point.Y) ? Y : point.Y);
        }

        public static bool AreModuleInBounds(Point m1, Point m2, Point bound1, Point bound2)
        {
            return m1 < bound2 && m1 > bound1 && m2 < bound2 && m2 > bound1;
            //return ((leftUpperPointM1.X <= leftUpperPointM2.X && leftUpperPointM2.X <= rightLowerPointM1.X ||
            //         leftUpperPointM2.X <= leftUpperPointM1.X && leftUpperPointM1.X <= rightLowerPointM2.X) &&
            //        (leftUpperPointM1.Y <= leftUpperPointM2.Y && leftUpperPointM2.Y <= rightLowerPointM1.Y ||
            //         leftUpperPointM2.Y <= leftUpperPointM1.Y && leftUpperPointM1.Y <= rightLowerPointM2.Y));
        }
    }
}
