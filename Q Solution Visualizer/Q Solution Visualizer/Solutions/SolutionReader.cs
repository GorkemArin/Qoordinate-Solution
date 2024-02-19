using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Solution_Visualizer.Solutions
{
    static class SolutionReader
    {
        public static Solution ReadSolution(string filePath)
        {
            string[] allLines = File.ReadAllLines(filePath);

            if (!File.Exists(filePath))
                return null;

            Solution solution = new Solution();

            foreach(string _line in allLines)
            {
                string line = _line.Trim();
                if (line.StartsWith(">"))
                {
                    ReadOrderLine(line.Substring(1).Trim(), solution);
                }
            }

            return solution;
        }

        private static void ReadOrderLine(string line, Solution solution)
        {
            int semicolonIndex = line.IndexOf(':');
            if (semicolonIndex == -1)
                return;

            string teamName = line.Substring(0, semicolonIndex);
            string orderStr = line.Substring(semicolonIndex + 1);
            string[] buildNames = orderStr.Split(',');

            for(int i = 0; i < buildNames.Length; i++)
            {
                buildNames[i] = RemoveQuotes(buildNames[i].Trim());
            }

            solution.AddNewOrder(teamName, buildNames);
        }

        private static string RemoveQuotes(string name)
        {
            if(name[0] == '"' && name[name.Length - 1] == '"')
            {
                return name.Substring(1, name.Length - 2);
            }

            return name;
        }
    }
}
