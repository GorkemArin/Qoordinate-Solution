using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Q_Solution_Visualizer
{
    class ListViewFunctions
    {
        ListView listView;
        public ListViewFunctions(ListView listView)
        {
            this.listView = listView;
        }

        //https://johnatten.com/2012/05/09/extending-c-listview-with-collapsible-groups-part-i/
        //Collapsible groups. Check
        public void InitializeListView()
        {
            listView.Clear();
            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;
            listView.Groups.Add(new ListViewGroup("harita", "Harita"));
            listView.Groups.Add(new ListViewGroup("ekip", "Ekipler"));
            listView.Groups.Add(new ListViewGroup("bina", "Binalar"));
            listView.Columns.Add("Ekip Adı", -2);
            listView.Columns.Add("Koordinatı", -2);
            listView.Columns.Add("Kapasitesi", -2);
            string[] itemlist = { "ekip1", "(12, 5)", "6" };
            ListViewItem item = new ListViewItem(itemlist, listView.Groups[1]);
            listView.Items.Add(item);
        }
    }
}
