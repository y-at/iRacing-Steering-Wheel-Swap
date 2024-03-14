using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace iRacing_Steering_Wheel_Swap
{
    public partial class Form1 : Form
    {   
        string iRacingDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\iRacing";
        string iRacingControlsDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacing\\controls\\");
        bool owstate = false;
        List<string> controlList = new List<string>();
        public Form1()
        {
            InitializeComponent();
            CreateDirectoryForActiveUser();
            populateWheelMenu();
        }

        private void CreateDirectoryForActiveUser()
        {
            // Check if the directory exists
            if (!Directory.Exists(iRacingControlsDirectoryPath))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(iRacingControlsDirectoryPath);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create wheel file name from textbox input
            string wheelName = newWheelName.Text + ".cfg";
            // Directories for orignial cfg and where custom cfg's are stored
            string originalControlPath = iRacingDirectoryPath + "\\controls.cfg";
            string newControlPath = iRacingControlsDirectoryPath + wheelName;
            // Draw message box if name field is empty
            if (newWheelName.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("File must have valid name.");
            }
            // Copy currently in use cfg to control directory
            File.Copy(originalControlPath, newControlPath, owstate);
            // Populate menu with pre-existing + newly added wheel
            populateWheelMenu();
        }

        private void populateWheelMenu()
        {
            // Get list of .cfg files in controls directory
            DirectoryInfo di = new DirectoryInfo(iRacingControlsDirectoryPath);
            FileInfo[] cfgFiles = di.GetFiles("*.cfg");
            // Clear pre-existing panels to avoid duplicates
            flowLayoutPanel1.Controls.Clear();
            controlList.Clear();
            // Give prompt in GUI if no cfg files exist
            if (cfgFiles.Length == 0)
            {   
                label1.Text = "No cfg files in directory.";
            }
            else {
                // Remove GUI no cfg prompt 
                label1.Text = "";
                // Iterate through total cfg files to draw panels
                for (int i = 0; i < cfgFiles.Length; i++)
                {
                    // Get cfg files from directory
                    controlList.Append(iRacingControlsDirectoryPath + cfgFiles[i]);
                    string wheelName = cfgFiles[i].Name;
                    // Panel characteristics
                    Panel panel;
                    panel = new Panel();
                    panel.Name = wheelName;
                    panel.BackColor = Color.DarkGray;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Size = new Size(120, 120);
                    panel.Margin = new Padding(10);
                    // Label of each panel
                    Label label;
                    label = new Label();
                    label.Name = wheelName;
                    label.Text = wheelName;
                    label.Location = new Point(0, 0);
                    label.ForeColor = Color.White;
                    label.AutoSize = true;
                    // Event handler used to load the double clicked config file
                    panel.DoubleClick += new EventHandler(Edit_DoubleClick);
                    foreach (Control c in panel.Controls)
                    {
                        c.DoubleClick += new EventHandler(Edit_DoubleClick);
                        Debug.WriteLine("Double click");
                    }
                    // Append label to panel
                    panel.Controls.Add(label);
                    // Add panels to flow panel
                    flowLayoutPanel1.Controls.Add(panel);
                }
                
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Toggle overwrite state
            if (owstate == true)
            {
                owstate = false;
                button1.Text = "Overwrite disabled";
            }
            else
            {
                owstate = true;
                button1.Text = "Overwrite enabled";
            }
        }

        private void Edit_DoubleClick(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            string chosenControlFile = (string)c.Name;
            Debug.WriteLine(chosenControlFile);
            File.Copy(iRacingControlsDirectoryPath+chosenControlFile, iRacingDirectoryPath+"\\controls.cfg", true);
            System.Windows.Forms.MessageBox.Show("Preset loaded.");
            Debug.WriteLine(iRacingControlsDirectoryPath + chosenControlFile);
            Debug.WriteLine(iRacingDirectoryPath + "\\" +chosenControlFile);
        }
    }
}
