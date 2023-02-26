using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Diagnostics;
using AngouriMath.Extensions;

namespace IntegralCalculator
{
    public static class TheMiddleRectangle
    {
        public static double Result { get; set; }
        public static PointPairList DataPoint { get; set; }
        public static PointPairList TheMiddleRectangleMethod(DataFunction function)
        {
            DataPoint = new PointPairList();
            Result = 0;
            for (int counter = 1; counter <= function.nodes; counter++)
            {
                var pointX = CountNodes(function, counter, function.nodeDistance);
                var pointY = CalcY(function, pointX);
                Result += function.nodeDistance * pointY;
                DataPoint.Add(pointX, pointY);
            }
            return DataPoint;
        }
        public static double CountNodes(DataFunction function, int counter, double nodeDistance)
        {
            return function.lowerBound + counter * nodeDistance - nodeDistance / 2;//x[i/2] = a + i*h - h/2
        }
        public static double CalcY(DataFunction function, double nodeValue)
        {
            return function.passedFunction.Compile(function.integrationVariable).Substitute(nodeValue).Real;//f(x[i])
        }
    }
}
