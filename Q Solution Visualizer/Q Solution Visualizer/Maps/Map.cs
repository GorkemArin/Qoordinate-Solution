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

        public void Rename(string name)
        {
            this.mapName = name;
        }

        public override string ToString()
        {
            return mapName;
        }

    }
}
