using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using AngouriMath.Extensions;

namespace IntegralCalculator
{
    class Simpson
    {
        public static double Result { get; set; }
        public static PointPairList DataPoint { get; set; }
        public static PointPairList SimpsonMethod(DataFunction function)
        {
            DataPoint = new PointPairList();
            Result = 0;
            for (int counter = 1; counter <= function.nodes; counter++)
            {
                var pointLeft = CountNodes(function, counter - 1, function.nodeDistance);
                var pointRight = CountNodes(function, counter, function.nodeDistance);
                var pointMiddle = (pointLeft + pointRight) / 2;
                var pointY1 = CalcY(function, pointLeft);
                var pointY2 = CalcY(function, pointMiddle);
                var pointY3 = CalcY(function, pointRight);
                Result += (pointY1 + 4 * pointY2 + pointY3) * function.nodeDistance / 6;
                DataPoint.Add(pointMiddle, pointY2);
                
            }
            return DataPoint;
        }
        public static double CountNodes(DataFunction function, int counter, double nodeDistance)
        {
            return function.lowerBound + counter * function.nodeDistance;
        }
        public static double CalcY(DataFunction function, double nodeValue)
        {
            return function.passedFunction.Compile(function.integrationVariable).Substitute(nodeValue).Real;//f(x[i])
        }
    }
}
