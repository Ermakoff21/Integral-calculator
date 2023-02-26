using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.Security;
using AngouriMath.Extensions;
using AngouriMath.Core;
using System.Drawing.Imaging;
using Label = System.Windows.Forms.Label;
using System.Net.Http;
using System.IO;
using System.Net;

namespace IntegralCalculator
{
    public partial class IntegralCalcForm : Form
    {
        HttpClient client = new HttpClient();   
        public IntegralCalcForm()
        {
            InitializeComponent();
            MinimumSize = new Size(1066, 600);
            GraphicPainter functionGraphic;
            DataFunction function;
            PointPairList pointRightPairList = new PointPairList();
            PointPairList pointPairMidList = new PointPairList();
            Color colorRightRect = Color.Red;
            Color colorMidRect = Color.Blue;
            Color colorSimpson = Color.Green;
            var mainFrame = new TableLayoutPanel()
            {
                RowCount = 1,
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };
            var graphicPanel = new ZedGraphControl()
            {
                Dock = DockStyle.Fill,
            };
            var controlPanel = new TableLayoutPanel() 
            { 
                Dock = DockStyle.Fill
            };
            var integralPanel = new TableLayoutPanel()
            {
                RowCount = 3,
                ColumnCount = 4,
                Dock = DockStyle.Fill
            };
            var settingsPanel = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 3,
                Dock = DockStyle.Fill
            };
            var buttonCalculate = new Button()
            {
                Text = "ВЫЧИСЛИТЬ",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(200, 40),
                Anchor = AnchorStyles.None,
            };
            var colorDialog = new ColorDialog();

            #region IntegralPanel
            var inputFunction = new TextBox()
            {
                Text = "Введите функцию",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 18),
                BorderStyle = BorderStyle.Fixed3D,
                TextAlign = HorizontalAlignment.Center,
                Anchor = AnchorStyles.Right,
                Size = ClientSize,
                ForeColor = Color.Gray
            };
            bool isFirstClick = true;
            var inputLowerBound = new TextBox()
            {
                Text = "pi/2",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BorderStyle = BorderStyle.Fixed3D,
                TextAlign = HorizontalAlignment.Center,
                Size = ClientSize,
                Anchor = AnchorStyles.Top

            };
            var inputUpperBound = new TextBox()
            {
                Text = "e",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BorderStyle = BorderStyle.Fixed3D,
                TextAlign = HorizontalAlignment.Center,
                Size = ClientSize,
                Anchor = AnchorStyles.Top,
            };
            var integrationVariable = new TextBox()
            {
                Text = "x",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 18),
                BorderStyle = BorderStyle.Fixed3D,
                TextAlign = HorizontalAlignment.Center,
                Anchor = AnchorStyles.None,
                Size = ClientSize
            };

            var differential = new Panel() 
            { 
                Anchor = AnchorStyles.Right, 
                Size = ClientSize 
            };
            differential.Paint += dDraw;
            differential.Resize += FormResize;

            var integralSymbol = new Panel() 
            { 
                Dock = DockStyle.Fill, 
                Size = ClientSize
            };
            integralSymbol.Paint += IntegralSymbolDraw;
            integralSymbol.Resize += FormResize;

            integralPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            integralPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            integralPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            integralPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            integralPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75));
            integralPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
            integralPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            
            integralPanel.Controls.Add(inputUpperBound, 0, 0);
            integralPanel.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 1, 0);
            integralPanel.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 0);
            integralPanel.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 3, 0);

            integralPanel.Controls.Add(integralSymbol, 0, 1);
            integralPanel.Controls.Add(inputFunction, 1, 1);
            integralPanel.Controls.Add(differential, 2, 1);
            integralPanel.Controls.Add(integrationVariable, 3, 1);

            integralPanel.Controls.Add(inputLowerBound, 0, 2);
            integralPanel.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 1, 2);
            integralPanel.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 2);
            integralPanel.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 3, 2);

            inputFunction.Click += (s, e) => 
            { 
                if (isFirstClick)
                {
                    inputFunction.Text = "";
                    isFirstClick = false;
                    inputFunction.ForeColor = Color.Black;
                }
            };
            #endregion

            #region SettingsPanel
            var colorPanelRightRect = new Button() { BackColor = Color.Red, Size = new Size(30, 30), Anchor = AnchorStyles.None };
            var colorPanelMiddleRect = new Button() { BackColor = Color.Blue, Size = new Size(30, 30), Anchor = AnchorStyles.None };
            var colorPanelSimpson = new Button() { BackColor = Color.Green, Size = new Size(30, 30), Anchor = AnchorStyles.None};
            var rightRectangleCheck = new CheckBox() 
            { 
                Text = "Метод правых прямоугольников", 
                Dock = DockStyle.Fill, 
                Font = new Font("Arial", 12, FontStyle.Bold) 
            };
            var middleRectangleCheck = new CheckBox() 
            { 
                Text = "Метод средних прямоугольников", 
                Dock = DockStyle.Fill, 
                Font = new Font("Arial", 12, FontStyle.Bold) 
            };
            var simpsonMethodCheck = new CheckBox() 
            { 
                Text = "Метод Симпсона", 
                Dock = DockStyle.Fill, 
                Font = new Font("Arial", 12, FontStyle.Bold) 
            };
            var answerRightRect = new TextBox()
            {
                Dock = DockStyle.Fill,
                Anchor = AnchorStyles.None,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "=0,0"
            };
            var answerMidRect = new TextBox() 
            { 
                Dock = DockStyle.Fill, 
                Anchor = AnchorStyles.None,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "=0,0"
            };
            var answerSimpson = new TextBox() 
            { 
                Dock = DockStyle.Fill, 
                Anchor = AnchorStyles.None,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "=0,0"
            };

            settingsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            settingsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            settingsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9));
            settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71));
            settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            settingsPanel.Controls.Add(colorPanelRightRect, 0, 0);
            settingsPanel.Controls.Add(rightRectangleCheck, 1, 0);
            settingsPanel.Controls.Add(answerRightRect, 2, 0);

            settingsPanel.Controls.Add(colorPanelMiddleRect, 0, 1);
            settingsPanel.Controls.Add(middleRectangleCheck, 1, 1);
            settingsPanel.Controls.Add(answerMidRect, 2, 1);

            settingsPanel.Controls.Add(colorPanelSimpson, 0, 2);
            settingsPanel.Controls.Add(simpsonMethodCheck, 1, 2);
            settingsPanel.Controls.Add(answerSimpson, 2, 2);

            functionGraphic = new GraphicPainter(graphicPanel);
            graphicPanel.GraphPane.Title.Text = "Графики функций";
            graphicPanel.GraphPane.XAxis.Title.Text = "X";
            graphicPanel.GraphPane.YAxis.Title.Text = "Y";

            rightRectangleCheck.SizeChanged += CheckBoxResizeFont;
            middleRectangleCheck.SizeChanged += CheckBoxResizeFont;
            simpsonMethodCheck.SizeChanged += CheckBoxResizeFont;

            buttonCalculate.Click += (s, e) =>
            {
                bool flag = inputLowerBound.Text != "" && inputUpperBound.Text != "" &&
                    inputFunction.Text != "" && integrationVariable.Text != "";
                double a = 0;
                double b = 0;
                try
                {
                    Parallel.Invoke(
                        () => { a = inputLowerBound.Text.Compile().Substitute().Real; },
                        () => { b = inputUpperBound.Text.Compile().Substitute().Real; }
                    );
                }
                catch { MessageBox.Show("Пределы введены некорректно", "Ошибка ввода", MessageBoxButtons.OK); }
                if (a <= b)
                {
                    if (flag && functionGraphic.Pane != null)
                    {
                        functionGraphic.Pane.CurveList.Clear();
                        function = new DataFunction(a, b, inputFunction.Text, integrationVariable.Text, 10);
                        var func = function.passedFunction.Compile(function.integrationVariable);
                        if (rightRectangleCheck.Checked == true)
                        {
                            CalculateClick(function, rightRectangleCheck.Text, functionGraphic,
                                colorRightRect, (x) => TheRightRectangle.TheRightRectangleMethod(x));
                            answerRightRect.Text = "=" + Convert.ToString(Math.Round(TheRightRectangle.Result, 4));
                        }
                        if (middleRectangleCheck.Checked == true)
                        {
                            CalculateClick(function, middleRectangleCheck.Text, functionGraphic,
                                colorMidRect, (x) => TheMiddleRectangle.TheMiddleRectangleMethod(x));
                            answerMidRect.Text = "=" + Convert.ToString(Math.Round(TheMiddleRectangle.Result, 4));
                        }
                        if (simpsonMethodCheck.Checked == true)
                        {
                            CalculateClick(function, simpsonMethodCheck.Text, functionGraphic,
                                colorSimpson, (x) => Simpson.SimpsonMethod(x));
                            answerSimpson.Text = "=" + Convert.ToString(Math.Round(Simpson.Result, 4));
                        }
                        //try
                        //{
                            
                        //}
                        //catch{ MessageBox.Show("Функция введена некорректно", "Ошибка ввода", MessageBoxButtons.OK); }
                    }
                }
                else MessageBox.Show("Нижний предел больше верхнего", "Ошибка ввода", MessageBoxButtons.OK);
            };

            rightRectangleCheck.Click += (s, e) =>
            {
                MethodCheckBoxChange(rightRectangleCheck, functionGraphic, graphicPanel, 
                    rightRectangleCheck.Text, colorRightRect, TheRightRectangle.DataPoint);
            };

            middleRectangleCheck.Click += (s, e) =>
            {
                MethodCheckBoxChange(middleRectangleCheck, functionGraphic, graphicPanel, 
                    middleRectangleCheck.Text, colorMidRect, TheMiddleRectangle.DataPoint);
            };
            simpsonMethodCheck.Click += (s, e) =>
            {
                MethodCheckBoxChange(simpsonMethodCheck, functionGraphic, graphicPanel,
                    simpsonMethodCheck.Text, colorSimpson, Simpson.DataPoint);
            };

            colorPanelRightRect.Click += (s, e) =>
            {
                colorRightRect = ColorChange(colorDialog, colorPanelRightRect, colorRightRect, rightRectangleCheck.Text, 
                    functionGraphic, graphicPanel, TheRightRectangle.DataPoint);
            };

            colorPanelMiddleRect.Click += (s, e) =>
            {
                colorMidRect = ColorChange(colorDialog, colorPanelMiddleRect, colorMidRect, middleRectangleCheck.Text, 
                    functionGraphic, graphicPanel, TheMiddleRectangle.DataPoint);
            };
            colorPanelSimpson.Click += (s, e) =>
            {
                colorSimpson = ColorChange(colorDialog, colorPanelSimpson, colorSimpson, simpsonMethodCheck.Text,
                    functionGraphic, graphicPanel, Simpson.DataPoint);
            };

            #endregion

            //Настройка ControlPanel
            controlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            controlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            controlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            controlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            controlPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            controlPanel.Controls.Add(integralPanel, 0, 0);
            controlPanel.Controls.Add(settingsPanel, 0, 1);
            controlPanel.Controls.Add(new Panel(), 0, 2);
            controlPanel.Controls.Add(buttonCalculate, 0, 3);
            //Настройка MainFrame
            mainFrame.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainFrame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            mainFrame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainFrame.Controls.Add(controlPanel, 0, 0);
            mainFrame.Controls.Add(graphicPanel, 1, 0);
            Controls.Add(mainFrame);
            
        }

        private void MethodCheckBoxChange(CheckBox methodCheck, GraphicPainter functionGraphic, ZedGraphControl graphicPanel, 
            string methodName, Color color, PointPairList pointPairs)
        {
            if (methodCheck.Checked == false)
                functionGraphic.DeleteGraphic(methodName);
            if (methodCheck.Checked == true)
                functionGraphic.DrawGraphic(pointPairs, methodName, color);
        }
        private Color ColorChange(ColorDialog colorDialog, Button colorPanel, Color color, string methodName, 
            GraphicPainter functionGraphic, ZedGraphControl graphicPanel, PointPairList pointPairs)
        {
            if (colorDialog.ShowDialog() == DialogResult.Cancel) return color;
            colorPanel.BackColor = colorDialog.Color;
            color = colorDialog.Color;
            if(graphicPanel.GraphPane.CurveList.Count != 0)
            {
                functionGraphic.DeleteGraphic(methodName);
                functionGraphic.DrawGraphic(pointPairs, methodName, color);
            }
            return color;
        }
        private void CalculateClick(DataFunction function, string methodName, GraphicPainter painter, 
            Color color, Func<DataFunction, PointPairList> method)
        {
            var formContent = new MultipartFormDataContent();
            painter.DrawGraphic(method(function), methodName, color);

            var vals = new Dictionary<string, string>()
            {
                {"file", "image.jpg"}
            };
            var tcontent = new FormUrlEncodedContent(vals);
            var file = File.ReadAllBytes("image.jpg");
            var fcontent = new ByteArrayContent(file);
            formContent.Add(tcontent, "data");
            formContent.Add(fcontent, "file");
            foreach(var b in formContent)
            {
                Console.WriteLine(b);
            }
            HttpResponseMessage a = client.PostAsync("http://localhost/readWrite.php", formContent).Result;
            var str = a.Content.ReadAsStringAsync();
        }
        private void CheckBoxResizeFont(object sender, EventArgs e)
        {
            var panel = sender as CheckBox;
            panel.Font = new Font("Arial", panel.Width * 0.035f, FontStyle.Bold);
        }

        private void FormResize(object sender, EventArgs eventArgs)
        {
            var p = sender as Panel;
            p.Invalidate();
            Paint += (s, args) =>
            args.Graphics.DrawString(
            "",
            new Font("Arial", p.Height * 0.63f),
            Brushes.Black,
            new Point(p.Width / 2, p.Height / 2),
            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
            );
        }

        private void IntegralSymbolDraw(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawString(
            "\u222B",
            new Font("Arial", p.Height * 0.62f, FontStyle.Bold),
            Brushes.Black,
            new Point(p.Width / 2, p.Height / 2 + (int)(p.Height * 0.05)),
            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
            );
        }
        private void dDraw(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawString(
            "d",
            new Font("Arial", 20, FontStyle.Bold),
            Brushes.Black,
            new Point(p.Width / 2, p.Height / 2 - 1),
            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
            );
        }
    }
}
