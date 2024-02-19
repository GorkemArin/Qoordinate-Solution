using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            foreach(string data in dataList)
            {
                if (data.StartsWith("mapname:"))
                {
                    string name = data.Substring(data.IndexOf(':') + 1).Trim();
                    if (name[0] == '"' && name[name.Length - 1] == '"')
                        name = name.Substring(1, name.Length - 2);
                    mapName = name;
                }
            }
        }
        private static void ReadBuildingData(string[] dataList)
        {
            string name = "";
            Tuple<double, double> coordinate = new Tuple<double, double>(0,0);
            double need = -1;

            foreach (string data in dataList)
            {
                if (data.StartsWith("name:"))
                {
                    name = data.Substring(data.IndexOf(':') + 1).Trim();
                    if(name[0] == '"' && name[name.Length - 1] == '"')
                        name = name.Substring(1, name.Length - 2);
                }
                else if(data.StartsWith("point:"))
                {
                    string pointStr = data.Substring(data.IndexOf(':') + 1).Trim();
                    string[] pointArr = pointStr.Split(',');
                    double x = Convert.ToDouble(pointArr[0].Trim());
                    double y = Convert.ToDouble(pointArr[1].Trim());
                    coordinate = new Tuple<double, double>(x, y);
                }
                else if (data.StartsWith("need:"))
                {
                    string needStr = data.Substring(data.IndexOf(':') + 1).Trim();
                    need = Convert.ToDouble(needStr.Trim());
                }
            }

            buildingsList.Add(new Building(name, coordinate, need));
        }
        private static void ReadTeamData(string[] dataList)
        {
            string name = "";
            Tuple<double, double> coordinate = new Tuple<double, double>(0, 0);
            double cap = -1;

            foreach (string data in dataList)
            {
                if (data.StartsWith("name:"))
                {
                    name = data.Substring(data.IndexOf(':') + 1).Trim();
                    if (name[0] == '"' && name[name.Length - 1] == '"')
                        name = name.Substring(1, name.Length - 2);
                }
                else if (data.StartsWith("point:"))
                {
                    string pointStr = data.Substring(data.IndexOf(':') + 1).Trim();
                    string[] pointArr = pointStr.Split(',');
                    double x = Convert.ToDouble(pointArr[0].Trim());
                    double y = Convert.ToDouble(pointArr[1].Trim());
                    coordinate = new Tuple<double, double>(x, y);
                }
                else if (data.StartsWith("cap:"))
                {
                    string capStr = data.Substring(data.IndexOf(':') + 1).Trim();
                    cap = Convert.ToDouble(capStr.Trim());
                }
            }

            teamsList.Add(new Team(name, coordinate, cap));
        }
        private static void ReadLine(string line)
        {
            if (line.Length == 0)
                return;

            if (line.StartsWith(">"))
            {
                line = line.Substring(1).Trim();

                if (line.StartsWith("map"))
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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
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

            return new Map(mapName, buildingsList, teamsList);
        }

    }
}
