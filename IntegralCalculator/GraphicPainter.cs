using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Net;

namespace IntegralCalculator
{
    class GraphicPainter
    {
        public ZedGraphControl GraphicPanel { get; set; }
        public GraphPane Pane { get; set; }
        public LineItem Line { get; set; }
        public GraphicPainter(ZedGraphControl panel)
        {
            GraphicPanel = panel;
            
        }
        public ZedGraphControl DrawGraphic(PointPairList pointPairs, string method, Color color)
        {
            Pane = GraphicPanel.GraphPane;
            Pane.AddCurve(method, pointPairs, color, SymbolType.None);
            GraphicPanel.AxisChange();
            GraphicPanel.Invalidate();
            Bitmap image = new Bitmap(GraphicPanel.Width, GraphicPanel.Height);
            GraphicPanel.DrawToBitmap(image, GraphicPanel.ClientRectangle);
            image.Save(@"C:\Users\HUAWEI\OneDrive\Рабочий стол\IntegralCalculator\IntegralCalculator\IntegralCalculator\bin\Debug\image.jpg", 
                ImageFormat.Jpeg);
            return GraphicPanel;
        }

        public void DeleteGraphic(string methodName)
        {
            CurveItem curveItem = null;
            if(Pane != null)
            {
                foreach (var item in Pane.CurveList)
                    if (item.Label.Text == methodName) curveItem = item;
                Pane.CurveList.Remove(curveItem);
                GraphicPanel.AxisChange();
                GraphicPanel.Invalidate();
            }
        }
    }
}
