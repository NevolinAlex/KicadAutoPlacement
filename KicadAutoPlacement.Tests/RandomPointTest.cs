using System;
using KicadAutoPlacement.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KicadAutoPlacement.Tests
{
    [TestClass]
    public class RandomPointTest
    {
        [TestMethod]
        public void RandomPointInRange()
        {
            for (int i = 0; i < 10; i++)
            {
                Point p1 = GeometricSolver.GetRandomPointInRange(400,400);
                Assert.IsTrue(p1.X<400 && p1.X > 0);
                Assert.IsTrue(p1.Y < 400 && p1.Y > 0);
            }
        }
    }
}
