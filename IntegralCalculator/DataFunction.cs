using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngouriMath.Extensions;
using AngouriMath.Core;
using System.Windows.Forms;
using AngouriMath.Core;

namespace IntegralCalculator
{
    public class DataFunction
    {
        public readonly double lowerBound;
        public readonly double upperBound;
        public readonly string integrationVariable;
        public readonly string passedFunction;
        public readonly int nodes;
        public readonly double nodeDistance;

        public DataFunction(double lowerBound, double upperBound, string passedFunction, string integrationVariable, int nodes)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            this.passedFunction = passedFunction;
            this.integrationVariable = integrationVariable;
            this.nodes = nodes;
            nodeDistance = (this.upperBound - this.lowerBound) / this.nodes;
        }
    }
}