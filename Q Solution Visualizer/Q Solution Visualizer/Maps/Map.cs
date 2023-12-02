using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Solution_Visualizer.Maps
{
    class Map
    {
        string mapName;
        List<Building> buildingsList;
        List<Team> teamsList;

        public Map(string mapName)
        {
            this.mapName = mapName;
            this.buildingsList = new List<Building>();
            this.teamsList = new List<Team>();
        }

        public Map(string mapName, List<Building> buildingsList, List<Team> teamsList)
        {
            this.mapName = mapName;
            this.buildingsList = buildingsList;
            this.teamsList = teamsList;
        }

        public Building[] GetBuildingsArray()
        {
            return buildingsList.ToArray();
        }

        public Team[] GetTeamsArray()
        {
            return teamsList.ToArray();
        }

        public string GetName()
        {
            return mapName;
        }

        public void AddTeam(Team team)
        {
            teamsList.Add(team);
        }

        public void AddBuilding(Building building)
        {
            buildingsList.Add(building);
        }

        public void RemoveTeamAt(int index)
        {
            if (teamsList.Count > index)
                teamsList.RemoveAt(index);
        }

        public void RemoveBuildingAt(int index)
        {
            if (buildingsList.Count > index)
                buildingsList.RemoveAt(index);
        }

        public void ReplaceTeamAt(Team newTeam, int index)
        {
            teamsList[index] = newTeam;
        }

        public void ReplaceBuildingAt(Building newBuilding, int index)
        {
            buildingsList[index] = newBuilding;
        }

        public void Rename(string name)
        {
            this.mapName = name;
        }

        public double[] GetExtremeCoordinates()
        {
            if(teamsList.Count + buildingsList.Count == 0)
            {
                return new double[]{-1, -1, 1, 1};
            }

            double minX = Double.MaxValue;
            double maxX = Double.MinValue;
            double minY = Double.MaxValue;
            double maxY = Double.MinValue;

            foreach(Team team in teamsList)
            {
                var coord = team.GetCoordinate();
                if (coord.Item1 < minX)
                    minX = coord.Item1;
                if (coord.Item1 > maxX)
                    maxX = coord.Item1;
                if (coord.Item2 < minY)
                    minY = coord.Item2;
                if (coord.Item2 > maxY)
                    maxY = coord.Item2;
            }

            foreach (Building building in buildingsList)
            {
                var coord = building.GetCoordinate();
                if (coord.Item1 < minX)
                    minX = coord.Item1;
                if (coord.Item1 > maxX)
                    maxX = coord.Item1;
                if (coord.Item2 < minY)
                    minY = coord.Item2;
                if (coord.Item2 > maxY)
                    maxY = coord.Item2;
            }

            return new double[] { minX, minY, maxX, maxY };
        }

        public override string ToString()
        {
            return mapName;
        }

    }
}
