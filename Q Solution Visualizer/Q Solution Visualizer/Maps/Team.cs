using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Solution_Visualizer.Maps
{
    class Team
    {
        string name;
        Tuple<double, double> coordinate;
        double capacity;

        public Team(string name, Tuple<double, double> coordinate, double capacity)
        {
            this.name = name;
            this.coordinate = coordinate;
            this.capacity = capacity;
        }

        public void SetVariables(string name = "", double x = Double.NaN, double y = Double.NaN, double capacity = Double.NaN)
        {
            double curX = coordinate.Item1;
            double curY = coordinate.Item2;

            if (name != "")
                this.name = name;
            if (x != Double.NaN)
                curX = x;
            if (y != Double.NaN)
                curY = y;
            if (capacity != Double.NaN)
                this.capacity = capacity;

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

        public double GetCapacity()
        {
            return capacity;
        }
    }
}
