using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KicadAutoPlacement;
using KicadAutoPlacement.GenAlgorithm;
namespace KicadAutoPlacement.Tests
{
    [TestClass]
    public class IntersectionTest
    {
        [TestMethod]
        public void IntersectedTest1()
        {
            var res = PrintedCircuitBoard.AreSegmentsIntersect(
                new Point(0, 0),
                new Point(5, 5),
                new Point(0, 5),
                new Point(5, 0));
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void NotIntersectedTest1()
        {
            var res = PrintedCircuitBoard.AreSegmentsIntersect(
                new Point(152.4, 133.35),
                new Point(205.7, 114.30),
                new Point(160, 140),
                new Point(180, 114));
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void IntersectedTest2()
        {
            var res = PrintedCircuitBoard.AreSegmentsIntersect(
                new Point(10, 10),
                new Point(100, 100),
                new Point(50, 0),
                new Point(50, 60));
            Assert.IsTrue(res);
        }

    }
}
