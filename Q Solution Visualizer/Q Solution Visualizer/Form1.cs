using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Q_Solution_Visualizer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        string selectedPath;
        string[] mapFilePaths;
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowse = new FolderBrowserDialog();

            if (selectedPath == null)
                folderBrowse.SelectedPath = Directory.GetCurrentDirectory();
            else
                folderBrowse.SelectedPath = selectedPath;

            folderBrowse.ShowDialog();
            selectedPath = folderBrowse.SelectedPath;
            mapFilePaths = ListMapFilesInFolder(selectedPath);

            comboBoxMaps.Items.Clear();
            foreach(string path in mapFilePaths)
            {
                comboBoxMaps.Items.Add(Path.GetFileName(path));
            }

            if (comboBoxMaps.Items.Count > 0)
                comboBoxMaps.SelectedIndex = 0;
        }

        private string[] ListMapFilesInFolder(string path)
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

            return allMaps.ToArray();
        }
    }
}
