using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AngouriMath.Extensions;
using System.Diagnostics;
using ZedGraph;

namespace IntegralCalculator
{
    public static class TheRightRectangle
    {
        public static double Result { get; set; }
        public static PointPairList DataPoint { get; set; }
        public static double[] DataPointX { get; set; }
        public static double[] DataPointY { get; set; }
        public static PointPairList TheRightRectangleMethod(DataFunction function)
        {
            Result = 0;
            DataPoint = new PointPairList();
            DataPointX = new double[function.nodes];
            DataPointY = new double[function.nodes];
            PointPair[] points = new PointPair[function.nodes];

            CalcDataPointX(DataPointX, function);
            CalcDataPointY(DataPointY, DataPointX, function);
            ToPointPair(points, DataPointY, DataPointX, function);
            
            for (int i = 0; i < DataPointY.Length; i++)
                Result += DataPointY[i] * function.nodeDistance;
            DataPoint.AddRange(points);
            return DataPoint;
        }
        public static double CountNodes(DataFunction function, int counter)
        {
            return function.lowerBound + (counter + 1) * function.nodeDistance;//x[i] = a + i*h
        }
        public static double[] CalcDataPointX(double[] dataPointX, DataFunction function)
        {
            Parallel.For(0, function.nodes, i =>
            {
                dataPointX[i] = CountNodes(function, i);
            });
            return dataPointX;
        }
        public static double[] CalcDataPointY(double[] dataPointY, double[] dataPointX, DataFunction function)
        {
            Parallel.For(0, function.nodes, i =>
            {
                dataPointY[i] = function.passedFunction.Compile(function.integrationVariable).Substitute(dataPointX[i]).Real;
            });
            return dataPointY;
        }
        public static PointPair[] ToPointPair(PointPair[] pointPair, double[] dataPointY, double[] dataPointX, DataFunction function)
        {
            Parallel.For(0, function.nodes, i =>
            {
                pointPair[i] = new PointPair(dataPointX[i], dataPointY[i]);
            });
            return pointPair;
        }
    }
}

