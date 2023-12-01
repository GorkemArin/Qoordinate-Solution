using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q_Solution_Visualizer.Maps;
using ZedGraph;

namespace Q_Solution_Visualizer
{
    class ZedGraphFunctions
    {
        ZedGraphControl zedGraph;
        public ZedGraphFunctions (ZedGraphControl zedGraph)
        {
            this.zedGraph = zedGraph;
            zedGraph.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zedGraph.GraphPane.YAxis.MajorGrid.IsVisible = true;
            //AddIcon(0, 0, 1, Properties.Resources.building, "A building");
            AutoScaleWithEqualAspect();

            /*TODO:*/
            //zedGraph.GraphPane.AxisChangeEvent += new GraphPane.AxisChangeEventHandler(AxisChanged);
        }

        private void AutoScaleWithEqualAspect()
        {
            GraphPane graphPane = zedGraph.GraphPane;

            // Calculate the average scale factor between the x and y axes
            double averageScaleFactor = (graphPane.XAxis.Scale.Max - graphPane.XAxis.Scale.Min) /
                                        (graphPane.YAxis.Scale.Max - graphPane.YAxis.Scale.Min);

            // Use AutoScale method to autoscale the graph
            graphPane.AxisChange();
            zedGraph.AxisChange();

            // Set the scale of both axes based on the average scale factor
            double xCenter = (graphPane.XAxis.Scale.Max + graphPane.XAxis.Scale.Min) / 2.0;
            double xHalfRange = (graphPane.XAxis.Scale.Max - graphPane.XAxis.Scale.Min) / 2.0;

            double yCenter = (graphPane.YAxis.Scale.Max + graphPane.YAxis.Scale.Min) / 2.0;
            double yHalfRange = xHalfRange / averageScaleFactor;

            graphPane.XAxis.Scale.Max = xCenter + xHalfRange;
            graphPane.XAxis.Scale.Min = xCenter - xHalfRange;

            graphPane.YAxis.Scale.Max = yCenter + yHalfRange;
            graphPane.YAxis.Scale.Min = yCenter - yHalfRange;

            zedGraph.Refresh();
        }

        private void AddIcon(double x, double y, double size, Image image, string text)
        {
            // Create an ImageObj at the specified (x, y) coordinates
            ImageObj icon = new ImageObj(image, x - size/2,  y + size/2, size, size)
            {
                ZOrder = ZOrder.A_InFront
            };

            TextObj textObj = new TextObj(text, x, y - size * 3 / 5, CoordType.AxisXYScale, AlignH.Center, AlignV.Center)
            {
                ZOrder = ZOrder.A_InFront
            };

            zedGraph.GraphPane.GraphObjList.Add(icon);
            zedGraph.GraphPane.GraphObjList.Add(textObj);
        }

        //minX, minY, maxX, maxY
        private double AutoscaleWithExtremePoints(double[] extremes)
        {
            double width = extremes[2] - extremes[0];
            double height = extremes[3] - extremes[1];
            double midPointX = (extremes[2] + extremes[0]) / 2;
            double midPointY = (extremes[3] + extremes[1]) / 2;

            double longside = width > height ? width : height;

            zedGraph.GraphPane.XAxis.Scale.Min = midPointX - longside / 2 * 1.1;
            zedGraph.GraphPane.XAxis.Scale.Max = midPointX + longside / 2 * 1.1;
            zedGraph.GraphPane.YAxis.Scale.Min = midPointY - longside / 2 * 1.1;
            zedGraph.GraphPane.YAxis.Scale.Max = midPointY + longside / 2 * 1.1;

            //zedGraph.GraphPane.YAxis.Scale.

            zedGraph.Refresh();

            return longside;
        }

        //private void AxisChanged(object sender, EventArgs e)
        //{

        //}

        public void VisualizeMap(Map map)
        {
            double[] extremes = map.GetExtremeCoordinates();
            Building[] buildings = map.GetBuildingsArray();
            Team[] teams = map.GetTeamsArray();

            double longside = AutoscaleWithExtremePoints(extremes);
            double sizeOfImages = longside / 8;

            zedGraph.GraphPane.Title.Text = map.GetName();

            zedGraph.GraphPane.GraphObjList.Clear();
            foreach (Building build in buildings)
            {
                var coord = build.GetCoordinate();
                string text = build.GetName() + String.Format(" ({0})", build.GetNeed());
                AddIcon(coord.Item1, coord.Item2, sizeOfImages, Properties.Resources.building, text);
            }

            foreach (Team team in teams)
            {
                var coord = team.GetCoordinate();
                string text = team.GetName() + String.Format(" ({0})", team.GetCapacity());
                AddIcon(coord.Item1, coord.Item2, sizeOfImages, Properties.Resources.team, text);
            }

            zedGraph.Refresh();
        }



    }
}
