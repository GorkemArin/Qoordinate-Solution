using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Q_Solution_Visualizer.Maps;


namespace Q_Solution_Visualizer
{
    public partial class FormMain : Form
    {
        ZedGraphFunctions zedFunctions;
        ListViewFunctions listViewFunctions;
        public FormMain()
        {
            InitializeComponent();
            zedFunctions = new ZedGraphFunctions(zedGraphControlMap);
            listViewFunctions = new ListViewFunctions(listViewTeamContent, listViewBuildingContent);

            listViewFunctions.InitializeListViewControls();

            selectedDirectory = Properties.Settings.Default.MapAdress;

            if(selectedDirectory != "" && Directory.Exists(selectedDirectory))
                ReadMapsFromDirectory();
        }

        string selectedDirectory;
        string previousSelectedDirectory;
        List<string> mapFilePaths;

        Map selectedMap;
        bool mapCustomized = false;
        bool customMapSaved = false;

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (!CheckCustomMapOkToContinue(ignoreSelected: true))
                return;
            
            FolderBrowserDialog folderBrowse = new FolderBrowserDialog();

            if (selectedDirectory == null || selectedDirectory == "")
                folderBrowse.SelectedPath = Directory.GetCurrentDirectory();
            else
                folderBrowse.SelectedPath = selectedDirectory;

            folderBrowse.ShowDialog();
            previousSelectedDirectory = selectedDirectory;
            selectedDirectory = folderBrowse.SelectedPath;
            ReadMapsFromDirectory();
        }

        private void ReadMapsFromDirectory(bool checkEmpty = true)
        {
            mapFilePaths = ListMapFilesInFolder(selectedDirectory);

            comboBoxMaps.Items.Clear();
            foreach (string path in mapFilePaths)
            {
                comboBoxMaps.Items.Add(Path.GetFileName(path));
            }

            if (comboBoxMaps.Items.Count > 0)
            {
                comboBoxMaps.SelectedIndex = 0;
                Properties.Settings.Default.MapAdress = selectedDirectory;
                Properties.Settings.Default.Save();
            }
            else if(checkEmpty)
            {
                DialogResult result = MessageBox.Show("Bu adreste hiçbir .map dosyası bulunamadı. " + 
                    "Devam edilsin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(result == DialogResult.No)
                {
                    selectedDirectory = previousSelectedDirectory;
                    ReadMapsFromDirectory(checkEmpty: false);
                }
            }
        }

        private List<string> ListMapFilesInFolder(string path)
        {
            List<string> allMaps = new List<string>();
            string[] allFiles = Directory.GetFiles(path);

            foreach(string fileName in allFiles)
            {
                if (fileName.EndsWith(".map"))
                {
                    allMaps.Add(fileName);
                }
            }

            return allMaps;
        }

        private void comboBoxMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!CheckCustomMapOkToContinue())
            {
                return;
            }

            selectedMap = MapReader.ReadMap(mapFilePaths[comboBoxMaps.SelectedIndex]);
            zedFunctions.VisualizeMap(selectedMap);
            listViewFunctions.ListMapElements(selectedMap);
            mapCustomized = false;
        }


        bool teamItemSelected = false;
        bool buildingItemSelected = false;
        private void listViewTeamContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            teamItemSelected = true;
            buildingItemSelected = false;
        }

        private void listViewBuildingContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            teamItemSelected = false;
            buildingItemSelected = true;
        }

        private void MapCustomized()
        {
            if (!mapCustomized)
            {
                mapCustomized = true;
                comboBoxMaps.Items.Add(".custom.map");
                mapFilePaths.Add(Path.Combine(selectedDirectory, ".custom.map"));
                comboBoxMaps.SelectedIndex = comboBoxMaps.Items.Count - 1; //select last item
                buttonSaveMap.Enabled = true;
            }

            customMapSaved = false;
        }

        private bool CheckCustomMapOkToContinue(bool ignoreSelected = false)
        {
            if (!ignoreSelected && comboBoxMaps.SelectedItem.ToString() == ".custom.map")
            {
                return false;
            }

            if (mapCustomized && !customMapSaved)
            {
                DialogResult result = MessageBox.Show("Harita kaydedilmedi. Eğer yeni harita seçerseniz" +
                    " değişiklikleriniz kaybolacak. Devam edilsin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                string lastElement = "";
                if (comboBoxMaps.Items.Count > 0)
                {
                    lastElement = comboBoxMaps.Items[comboBoxMaps.Items.Count - 1].ToString();
                }
                if (result == DialogResult.No)
                {
                    if (lastElement != ".custom.map")
                    {
                        comboBoxMaps.Items.Add(".custom.map");
                    }
                    comboBoxMaps.SelectedIndex = comboBoxMaps.Items.Count - 1;
                    return false;
                }

                mapCustomized = false;
                customMapSaved = false;
                if (lastElement == ".custom.map")
                {
                    comboBoxMaps.Items.RemoveAt(comboBoxMaps.Items.Count - 1);
                }
            }

            return true;
        }

        private void SaveCustomMap()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Map Dosyası (.map)|*.map";
            saveFileDialog.InitialDirectory = selectedDirectory;
            saveFileDialog.ShowDialog();

            string path = saveFileDialog.FileName;

            MapWriter.WriteMap(selectedMap, path);
            mapCustomized = false;
            customMapSaved = true;
            buttonSaveMap.Enabled = false;

            selectedDirectory = Directory.GetParent(path).FullName;
            ReadMapsFromDirectory();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (teamItemSelected)
            {
                int index = listViewFunctions.GetSelectedTeamIndex();
                if (index == -1)
                    return;
                selectedMap.RemoveTeamAt(index);
                listViewFunctions.DeleteTeamAtSelectedIndex();
            }
            else if (buildingItemSelected)
            {
                int index = listViewFunctions.GetSelectedBuildingIndex();
                if (index == -1)
                    return;
                selectedMap.RemoveBuildingAt(index);
                listViewFunctions.DeleteBuildingAtSelectedIndex();
            }

            if (teamItemSelected || buildingItemSelected)
            {
                MapCustomized();
                zedFunctions.VisualizeMap(selectedMap);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (teamItemSelected)
            {
                int index = listViewFunctions.GetSelectedTeamIndex();
                if (index == -1)
                    return;
                Team selectedTeam = selectedMap.GetTeamsArray()[index];
                AddReplaceDialog addReplaceDialog = new AddReplaceDialog(selectedTeam);
                DialogResult result = addReplaceDialog.ShowDialog();
                if (result == DialogResult.Cancel)
                    return;

                selectedMap.ReplaceTeamAt(addReplaceDialog.newTeam, index);
                listViewFunctions.ReplaceTeamAt(addReplaceDialog.newTeam, index);
            }
            else if (buildingItemSelected)
            {
                int index = listViewFunctions.GetSelectedBuildingIndex();
                if (index == -1)
                    return;
                Building selectedBuild = selectedMap.GetBuildingsArray()[index];
                AddReplaceDialog addReplaceDialog = new AddReplaceDialog(selectedBuild);
                DialogResult result = addReplaceDialog.ShowDialog();
                if (result == DialogResult.Cancel)
                    return;

                selectedMap.ReplaceBuildingAt(addReplaceDialog.newBuilding, index);
                listViewFunctions.ReplaceBuildingAt(addReplaceDialog.newBuilding, index);
            }

            if (teamItemSelected || buildingItemSelected)
            {
                MapCustomized();
                zedFunctions.VisualizeMap(selectedMap);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddReplaceDialog addReplaceDialog = new AddReplaceDialog();
            DialogResult result = addReplaceDialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return;

            if (addReplaceDialog.teamOrBuilding)
            {
                selectedMap.AddBuilding(addReplaceDialog.newBuilding);
                listViewFunctions.AddBuilding(addReplaceDialog.newBuilding);
            }
            else
            {
                selectedMap.AddTeam(addReplaceDialog.newTeam);
                listViewFunctions.AddTeam(addReplaceDialog.newTeam);
            }

            MapCustomized();
            zedFunctions.VisualizeMap(selectedMap);
        }

        private void buttonSaveMap_Click(object sender, EventArgs e)
        {
            if(mapCustomized && !customMapSaved)
            {
                SaveCustomMap();
            }
        }

        private void buttonSolveMap_Click(object sender, EventArgs e)
        {
            string directory = @"C:\gorkemarin\Qoordinate\My Solution\";
            string pythonfile = Path.Combine(directory, "QSolveProblem.py");
            string map = Path.Combine(directory, @"Maps\test0.map");
            string sol = Path.Combine(directory, @"Maps\test0_3.sol");
            string solver = "BruteForce";

            Process proc = Process.Start("python", String.Format("\"{0}\" \"{1}\" {2} \"{3}\"", pythonfile, map, solver, sol));
            proc.WaitForExit();
            TimeSpan time = proc.TotalProcessorTime;
        }
    }
}
