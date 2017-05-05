using System;
using KicadAutoPlacement.GenAlgorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KicadAutoPlacement.Tests
{
    [TestClass]
    public class RotateTests
    {
        [TestMethod]
        public void RotateTest1()
        {
            Point point = Chromosome.RotatePoint(new Point(0,1), Math.PI/2);
            double x = 1;
            double y = 0;
            Assert.AreEqual(point.X, x, 10e-6);
            Assert.AreEqual(point.Y, y, 10e-6);
        }

        [TestMethod]
        public void RotateTest2()
        {
            Point point = Chromosome.RotatePoint(new Point(1, 0), Math.PI / 2);
            double x = 0;
            double y = -1;
            Assert.AreEqual(point.X, x, 10e-6);
            Assert.AreEqual(point.Y, y, 10e-6);
        }

        [TestMethod]
        public void RotateTest3()
        {
            Point point = Chromosome.RotatePoint(new Point(0, -1), Math.PI / 2);
            double x = -1;
            double y = 0;
            Assert.AreEqual(point.X, x, 10e-6);
            Assert.AreEqual(point.Y, y, 10e-6);
        }
    }
}
