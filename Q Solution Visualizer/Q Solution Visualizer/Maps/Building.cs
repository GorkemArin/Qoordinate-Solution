using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Solution_Visualizer.Maps
{
    public class Building
    {
        string name;
        Tuple<double, double> coordinate;
        double need;

        public Building(string name, Tuple<double, double> coordinate, double need)
        {
            this.name = name;
            this.coordinate = coordinate;
            this.need = need;
        }

        public void SetVariables(string name = "", double x = Double.NaN, double y = Double.NaN, double need = Double.NaN)
        {
            double curX = coordinate.Item1;
            double curY = coordinate.Item2;

            if (name != "")
                this.name = name;
            if (x != Double.NaN)
                curX = x;
            if (y != Double.NaN)
                curY = y;
            if (need != Double.NaN)
                this.need = need;

            coordinate = new Tuple<double, double>(curX, curY);
        }

        public string GetName()
        {
            return name;
        }

        public Tuple<double, double> GetCoordinate()
        {
            return coordinate;
        }

        public double GetNeed()
        {
            return need;
        }

    }
}
