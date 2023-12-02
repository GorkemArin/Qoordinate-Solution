using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Q_Solution_Visualizer.Maps;

namespace Q_Solution_Visualizer
{
    public partial class AddReplaceDialog : Form
    {
        //false: team, true: build
        public bool teamOrBuilding = false;
        public Building newBuilding;
        public Team newTeam;

        public AddReplaceDialog()
        {
            InitializeComponent();

            radioButtonTeam.Checked = true;
            radioButton_CheckedChanged(null, null);
            buttonAddReplace.Text = "Ekle";
            this.Text = "Ekle";
        }

        public AddReplaceDialog(Team team)
        {
            InitializeComponent();

            radioButtonTeam.Checked = true;
            radioButtonBuilding.Enabled = false;
            radioButton_CheckedChanged(null, null);
            buttonAddReplace.Text = "Değiştir";
            this.Text = "Değiştir";
            textBoxTeamName.Text = team.GetName();
            textBoxTeamX.Text = team.GetCoordinate().Item1.ToString();
            textBoxTeamY.Text = team.GetCoordinate().Item2.ToString();
            textBoxTeamCap.Text = team.GetCapacity().ToString();
        }

        public AddReplaceDialog(Building building)
        {
            InitializeComponent();

            radioButtonBuilding.Checked = true;
            radioButton_CheckedChanged(null, null);
            radioButtonTeam.Enabled = false;
            buttonAddReplace.Text = "Değiştir";
            this.Text = "Değiştir";
            textBoxBuildName.Text = building.GetName();
            textBoxBuildX.Text = building.GetCoordinate().Item1.ToString();
            textBoxBuildY.Text = building.GetCoordinate().Item2.ToString();
            textBoxBuildNeed.Text = building.GetNeed().ToString();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxTeam.Enabled = radioButtonTeam.Checked;
            groupBoxBuilding.Enabled = radioButtonBuilding.Checked;
        }

        private Team GetTeam()
        {
            string name = textBoxTeamName.Text;
            double x = Convert.ToDouble(textBoxTeamX.Text);
            double y = Convert.ToDouble(textBoxTeamY.Text);
            double cap = Convert.ToDouble(textBoxTeamCap.Text);

            Team team = new Team(name, new Tuple<double, double>(x, y), cap);
            return team;
        }

        private Building GetBuilding()
        {
            string name = textBoxBuildName.Text;
            double x = Convert.ToDouble(textBoxBuildX.Text);
            double y = Convert.ToDouble(textBoxBuildY.Text);
            double need = Convert.ToDouble(textBoxBuildNeed.Text);

            Building build = new Building(name, new Tuple<double, double>(x, y), need);
            return build;
        }

        private void buttonAddReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonTeam.Checked)
                { //Team
                    teamOrBuilding = false;
                    newTeam = GetTeam();
                }
                else
                { //Building
                    teamOrBuilding = true;
                    newBuilding = GetBuilding();
                }
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Takım/Ekip oluştururken hata meydana geldi:\n\n" + ex.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
