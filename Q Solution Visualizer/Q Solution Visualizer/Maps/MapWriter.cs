using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Solution_Visualizer.Maps
{
    static class MapWriter
    {
        public static void WriteMap(Map map, string path)
        {
            List<string> linesList = new List<string>();

            linesList.Add("> map:");
            linesList.Add("\t* mapname: " + '"' + map.GetName() + '"');
            linesList.Add("");

            linesList.Add("> buildings:");
            foreach (Building building in map.GetBuildingsArray())
            {
                string name = building.GetName();
                var coordinate = building.GetCoordinate();
                double need = building.GetNeed();

                string line = String.Format("\t* name: \"{0}\" | point: {1}, {2} | need: {3}",
                    name, coordinate.Item1, coordinate.Item2, need);

                linesList.Add(line);
            }

            linesList.Add("");
            linesList.Add("> teams:");
            foreach(Team team in map.GetTeamsArray())
            {
                string name = team.GetName();
                var coordinate = team.GetCoordinate();
                double cap = team.GetCapacity();

                string line = String.Format("\t* name: \"{0}\" | point: {1}, {2} | cap: {3}",
                    name, coordinate.Item1, coordinate.Item2, cap);

                linesList.Add(line);
            }

            string[] allLines = linesList.ToArray();
            Directory.CreateDirectory(Directory.GetParent(path).FullName);
            File.WriteAllLines(path, allLines);
        }

    }
}
