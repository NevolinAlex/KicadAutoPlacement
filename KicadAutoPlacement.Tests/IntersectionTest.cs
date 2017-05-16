using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KicadAutoPlacement;
using KicadAutoPlacement.GenAlgorithm;
using KicadAutoPlacement.Solvers;

namespace KicadAutoPlacement.Tests
{
    [TestClass]
    public class IntersectionTest
    {
        #region Segments
        [TestMethod]
        public void SegmentIntersctionTest1()
        {
            var res = GeometricSolver.AreSegmentsIntersect(
                new Point(0, 0),
                new Point(5, 5),
                new Point(0, 5),
                new Point(5, 0));
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void SegmentIntersctionTest2()
        {
            var res = GeometricSolver.AreSegmentsIntersect(
                new Point(152.4, 133.35),
                new Point(205.7, 114.30),
                new Point(160, 140),
                new Point(180, 114));
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void SegmentIntersctionTest3()
        {
            var res = GeometricSolver.AreSegmentsIntersect(
                new Point(10, 10),
                new Point(100, 100),
                new Point(50, 0),
                new Point(50, 60));
            Assert.IsTrue(res);
        }
        #endregion

        #region Rectangles
        [TestMethod]
        public void RectaglesIntersectionTest1()
        {
            var res = GeometricSolver.AreModuleInBounds(
                new Point(0, 0),
                new Point(10, 10),
                new Point(10, 10),
                new Point(20, 20));
            Assert.IsFalse(res);
        }
        [TestMethod]
        public void RectaglesIntersectionTest2()
        {
            var res = GeometricSolver.AreModuleInBounds(
                new Point(0, 0),
                new Point(10, 10),
                new Point(11, 11),
                new Point(20, 20));
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void RectaglesIntersectionTest3()
        {
            var res = GeometricSolver.AreModuleInBounds(
                new Point(0, 0),
                new Point(10, 10),
                new Point(5, 5),
                new Point(20, 20));
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void RectaglesIntersectionTest4()
        {
            var res = GeometricSolver.AreModuleInBounds(
                new Point(0, 0),
                new Point(10, 10),
                new Point(-5, -5),
                new Point(20, 20));
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void RectaglesIntersectionTest5()
        {
            var res = GeometricSolver.AreModuleInBounds(
                new Point(0, 0),
                new Point(10, 10),
                new Point(9, 9),
                new Point(20, 20));
            Assert.IsFalse(res);
        }


        #endregion


    }
}
