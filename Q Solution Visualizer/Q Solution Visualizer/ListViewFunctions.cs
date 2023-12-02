using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Q_Solution_Visualizer.Maps;

namespace Q_Solution_Visualizer
{
    class ListViewFunctions
    {
        ListView listViewTeams;
        ListView listViewBuildings;
        public ListViewFunctions(ListView listViewTeams, ListView listViewBuildings)
        {
            this.listViewTeams = listViewTeams;
            this.listViewBuildings = listViewBuildings;
        }

        public void InitializeListViewControls()
        {
            listViewTeams.Clear();
            listViewTeams.View = View.Details;
            listViewTeams.GridLines = true;
            listViewTeams.FullRowSelect = true;
            listViewTeams.Columns.Add("Ekip Adı", -2);
            listViewTeams.Columns.Add("Koordinatı", -2);
            listViewTeams.Columns.Add("Kapasitesi", -2);
            string[] itemlist = { "ekip1", "(12, 5)", "6" };
            ListViewItem item = new ListViewItem(itemlist);
            listViewTeams.Items.Add(item);

            listViewBuildings.Clear();
            listViewBuildings.View = View.Details;
            listViewBuildings.GridLines = true;
            listViewBuildings.FullRowSelect = true;
            listViewBuildings.Columns.Add("Bina Adı", -2);
            listViewBuildings.Columns.Add("Koordinatı", -2);
            listViewBuildings.Columns.Add("İhtiyacı", -2);
            string[] itemlist2 = { "Bina A", "(10, 0)", "10" };
            ListViewItem item2 = new ListViewItem(itemlist2);
            listViewBuildings.Items.Add(item2);
        }

        public void ListMapElements(Map map)
        {
            listViewTeams.Items.Clear();
            listViewBuildings.Items.Clear();

            foreach(Team team in map.GetTeamsArray())
            {
                string name = team.GetName();
                var coord = team.GetCoordinate();
                string coordinate = String.Format("({0},{1})", coord.Item1, coord.Item2);
                string cap = team.GetCapacity().ToString();
                ListViewItem item = new ListViewItem(new string[] { name, coordinate, cap });
                listViewTeams.Items.Add(item);
            }

            foreach (Building build in map.GetBuildingsArray())
            {
                string name = build.GetName();
                var coord = build.GetCoordinate();
                string coordinate = String.Format("({0},{1})", coord.Item1, coord.Item2);
                string need = build.GetNeed().ToString();
                ListViewItem item = new ListViewItem(new string[] { name, coordinate, need });
                listViewBuildings.Items.Add(item);
            }
        }

        public void DeleteTeamAtSelectedIndex()
        {
            if(listViewTeams.SelectedIndices.Count > 0)
                listViewTeams.Items.RemoveAt(listViewTeams.SelectedIndices[0]);
        }

        public void DeleteBuildingAtSelectedIndex()
        {
            if (listViewBuildings.SelectedIndices.Count > 0)
                listViewBuildings.Items.RemoveAt(listViewBuildings.SelectedIndices[0]);
        }
   
        public void ReplaceTeamAt(Team team, int index)
        {
            string name = team.GetName();
            var coord = team.GetCoordinate();
            string coordinate = String.Format("({0},{1})", coord.Item1, coord.Item2);
            string cap = team.GetCapacity().ToString();
            ListViewItem item = new ListViewItem(new string[] { name, coordinate, cap });
            listViewTeams.Items[index] = item;
        }

        public void ReplaceBuildingAt(Building build, int index)
        {
            string name = build.GetName();
            var coord = build.GetCoordinate();
            string coordinate = String.Format("({0},{1})", coord.Item1, coord.Item2);
            string need = build.GetNeed().ToString();
            ListViewItem item = new ListViewItem(new string[] { name, coordinate, need });
            listViewBuildings.Items[index] = item;
        }

        public void AddTeam(Team team)
        {
            string name = team.GetName();
            var coord = team.GetCoordinate();
            string coordinate = String.Format("({0},{1})", coord.Item1, coord.Item2);
            string cap = team.GetCapacity().ToString();
            ListViewItem item = new ListViewItem(new string[] { name, coordinate, cap });
            listViewTeams.Items.Add(item);
        }

        public void AddBuilding(Building build)
        {
            string name = build.GetName();
            var coord = build.GetCoordinate();
            string coordinate = String.Format("({0},{1})", coord.Item1, coord.Item2);
            string need = build.GetNeed().ToString();
            ListViewItem item = new ListViewItem(new string[] { name, coordinate, need });
            listViewBuildings.Items.Add(item);
        }

        public int GetSelectedTeamIndex()
        {
            if (listViewTeams.SelectedIndices.Count == 0)
                return -1;

            return listViewTeams.SelectedIndices[0];
        }

        public int GetSelectedBuildingIndex()
        {
            if (listViewBuildings.SelectedIndices.Count == 0)
                return -1;

            return listViewBuildings.SelectedIndices[0];
        }
    }
}
