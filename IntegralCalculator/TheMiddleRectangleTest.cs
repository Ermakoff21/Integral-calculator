using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegralCalculator
{
    [TestFixture]
    class TheMiddleRectangleTest
    {
        [TestCase("0", -1, 8, "y", 10, 0)]
        [TestCase("5", 0, 0, "x", 10, 0)]
        [TestCase("5", 3, 20, "x", 10, 85)]
        [TestCase("(cos(0.3 * x + 0.8))/(0.9 + 2 * sin(0.4 * x + 0.3))", 0.2, 1, "x", 10, 0.234610)]
        [TestCase("sqrt(1.7 * x^2 + 0.5)/(1.4 + sqrt(1.2 * x + 1.3))", 0.7, 2.1, "x", 10, 0.874808)]
        public void IntegralTest(string passedFunction, double lowerBound, double upperBound,
            string integrationVariable, int nodes, double expectedResult)
        {
            DataFunction function = new DataFunction(lowerBound, upperBound, passedFunction, integrationVariable, nodes);
            TheMiddleRectangle.TheMiddleRectangleMethod(function);
            var result = TheMiddleRectangle.Result;
            Assert.AreEqual(expectedResult, result, 1e-6);
        }
    }
}
