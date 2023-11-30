using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Solution_Visualizer.Maps
{
    enum Context
    {
        None = 0,
        Map,
        Buildings,
        Teams
    }

    static class MapReader
    {
        private static Context context;
        private static string mapName;
        private static Map map;
        private static List<Building> buildingsList;
        private static List<Team> teamsList;

        private static string[] SplitData(string line)
        {
            string[] dataList = line.Split('|');
            for (int i = 0; i < dataList.Length; i++)
            {
                dataList[i] = dataList[i].Trim();
            }
            return dataList;
        }

        private static void ReadMapData(string[] dataList)
        {

        }
        private static void ReadBuildingData(string[] dataList)
        {

        }
        private static void ReadTeamData(string[] dataList)
        {

        }

        private static void ReadLine(string line)
        {
            if (line.Length == 0)
                return;

            if (line.StartsWith(">"))
            {
                line = line.Substring(1).Trim();

                if (line.StartsWith("buildings"))
                    context = Context.Map;
                else if (line.StartsWith("buildings"))
                    context = Context.Buildings;
                else if (line.StartsWith("teams"))
                    context = Context.Teams;
                else
                    context = Context.None;
            }
            else if (line.StartsWith("*"))
            {
                line = line.Substring(1).Trim();

                string[] dataList = SplitData(line);
                if (context == Context.Map)
                    ReadMapData(dataList);
                else if (context == Context.Teams)
                    ReadTeamData(dataList);
                else if (context == Context.Buildings)
                    ReadBuildingData(dataList);
            }
        }

        public static Map ReadMap(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"{path} adresinde bir dosya bulunamadı.");
            }

            string[] lines = File.ReadAllLines(path);
            context = Context.None;
            buildingsList = new List<Building>();
            teamsList = new List<Team>();
            map = null;
            mapName = "";

            foreach(string line in lines)
            {
                ReadLine(line.Trim());
            }

            return null;
        }

    }
}
